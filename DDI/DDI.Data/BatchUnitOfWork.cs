using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;

namespace DDI.Data
{
    /// <summary>
    /// Class that allows processing a large number of entities in batches.  (Each batch uses a new UnitOfWork.)
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class BatchUnitOfWork<T> : IDisposable, IEnumerable<T> where T : class, IEntity
    {
        #region Private Fields

        private const int DEFAULT_BATCH_SIZE = 100;

        private List<Guid> _ids;
        private Expression<Func<T, object>>[] _includes;
        private bool _loaded = false;
        private int _skip;
        private UnitOfWorkEF _initialUnitOfWork = null;
        private IQueryable<T> _query = null;
        private List<ISorter> _sorters = null;
        private int _count;

        #endregion

        #region Public Properties

        /// <summary>
        /// UnitOfWork for the current batch.
        /// </summary>
        public UnitOfWorkEF UnitOfWork { get; private set; }

        /// <summary>
        /// Number of entities per batch.
        /// </summary>
        public int BatchSize { get; set; }

        /// <summary>
        /// Number of batches to skip
        /// </summary>
        public int BatchesToSkip { get; set; }

        /// <summary>
        /// Action to perform at the start of each batch.  This action is useful for creating businses logic classes or reporting progress.  
        /// It can also be used for enumerating each batch separately by calling the Process method instead of enumerating the BatchUnitOfWork.
        /// </summary>
        public Action<int,IEnumerable<T>> OnNextBatch { get; set; }

        /// <summary>
        /// Action to perform when all entities have been processed.
        /// </summary>
        public Action OnCompletion { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new BatchUnitOfWork.
        /// </summary>
        /// <param name="includes">Paths to include for the entity.</param>
        public BatchUnitOfWork(params Expression<Func<T, object>>[] includes)
        {
            BatchSize = DEFAULT_BATCH_SIZE;
            OnNextBatch = null;
            OnCompletion = null;
            _includes = includes;
            _initialUnitOfWork = new UnitOfWorkEF();  // The UnitOfWork to use for building the list of Ids.
            _query = _initialUnitOfWork.GetEntities<T>();  // The initial query to get all entities.  This can be filtered via Where.
            _sorters = new List<ISorter>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Filters entities based on a predicate.
        /// </summary>
        /// <param name="where">A function to test each entity for a condition.</param>
        /// <returns>The updated BatchUnitOfWork.</returns>
        public BatchUnitOfWork<T> Where(Expression<Func<T, bool>> where)
        {
            _query = _query.Where(where);
            return this;
        }

        /// <summary>
        /// Sorts entities in ascending order according to a key.
        /// </summary>
        /// <typeparam name="Tkey">The type of the key returned by the function that is represented by keySelector.</typeparam>
        /// <param name="orderBy">A function to extract a key from an entity.</param>
        /// <returns>The updated BatchUnitOfWork.</returns>
        public BatchUnitOfWork<T> OrderBy<Tkey>(Expression<Func<T,Tkey>> orderBy)
        {
            _query = _query.OrderBy(orderBy);
            _sorters.Add(new Sorter<Tkey>(orderBy, false)); // Capture the expression so batches can be ordered.
            return this;
        }

        /// <summary>
        /// Sorts entities in descending order according to a key.
        /// </summary>
        /// <typeparam name="Tkey">The type of the key returned by the function that is represented by keySelector.</typeparam>
        /// <param name="orderBy">A function to extract a key from an entity.</param>
        /// <returns>The updated BatchUnitOfWork.</returns>
        public BatchUnitOfWork<T> OrderByDescending<Tkey>(Expression<Func<T, Tkey>> orderBy)
        {
            _query = _query.OrderByDescending(orderBy);
            _sorters.Add(new Sorter<Tkey>(orderBy, true)); // Capture the expression so batches can be ordered.
            return this;
        }
       
        /// <summary>
        /// Returns an enumerator that enumerates entities in batches.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            if (BatchSize <= 0)
            {
                throw new InvalidOperationException("Batch size must be greater than zero.");
            }

            Load();

            while (true)
            {
                // Get a batch of id's.
                var idSet = _ids.Skip(_skip).Take(BatchSize);

                // If there are no more, exit the loop.
                if (idSet.Count() == 0)
                {
                    break;
                }

                // Create a UnitOfWork for this batch.
                using (UnitOfWork = new UnitOfWorkEF())
                {

                    // Create a EF set of entities for this batch of Id's.
                    IQueryable<T> entities = UnitOfWork.GetEntities<T>(_includes).Where(p => idSet.Contains(p.Id)); // This EF "Where" pattern provides a list of Ids to the SELECT statement.

                    // Apply any sorters to the query.
                    foreach (var sorter in _sorters)
                    { 
                        entities = sorter.Sort(entities);
                    }

                    // Invoke the OnNextBatch action.
                    OnNextBatch?.Invoke(_count, entities);

                    // Iterate through each entity in the query.
                    foreach (var entity in entities.Where(p => p != null))
                    {
                        yield return entity;
                    }

                    // Update the count and skip values.
                    _skip += BatchSize;
                    _count++;
                }
            }

            // Invoke the OnCompletion action.
            OnCompletion?.Invoke();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Process all entities, invoking the OnNextBatch action for each batch.
        /// </summary>
        /// <returns>Total number of entities procesed.</returns>
        public int Process()
        {
            return this.Count();
        }

        /// <summary>
        /// Releases managed resources used by the BatchUnitOfWork.
        /// </summary>
        public void Dispose()
        {
            UnitOfWork?.Dispose();
            _initialUnitOfWork?.Dispose();            

            UnitOfWork = null;
            _initialUnitOfWork = null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Logic to load the Ids of all selected entities into a list.
        /// </summary>
        private void Load()
        {
            if (!_loaded)
            {
                // Build the complete list of Ids to be processed.  The query should have been already filtered and ordered by Where(), OrderBy(), etc.
                _ids = _query.Select(p => p.Id).ToList();

                _count = 0;
                _skip = BatchesToSkip * BatchSize;
                _loaded = true;
                _initialUnitOfWork.Dispose();
                _initialUnitOfWork = null;
            }
        }

        #endregion

        #region Internal Classes and Interfaces

        /// <summary>
        /// Class to provide OrderBy functionality. The initial set of Ids may be ordered, but the batched sets must also be ordered.  
        /// This class captures the expressions used for OrderBy and OrderByDescending.
        /// </summary>
        private class Sorter<TKey> : ISorter
        {
            private Expression<Func<T, TKey>> _sortExpression = null;
            private bool _descending = false;

            public Sorter(Expression<Func<T, TKey>> sortExpression, bool descending)
            {
                _sortExpression = sortExpression;
                _descending = descending;
            }

            /// <summary>
            /// Sort an IQueryable using the captured expression.
            /// </summary>
            public IOrderedQueryable<T> Sort(IQueryable<T> source)
            {
                return _descending ? source.OrderByDescending(_sortExpression) : source.OrderBy(_sortExpression);
            }
        }

        /// <summary>
        /// Interface for Sorter that hides the key selector type parameter.
        /// </summary>
        private interface ISorter
        {
            IOrderedQueryable<T> Sort(IQueryable<T> source);
        }

        #endregion

    }

}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Data
{
    /// <summary>
    /// Unit of Work for tests - Manages a set of entity repositories added via SetRepository&lt;T&gt;(IRepository&lt;T&gt; repo)
    /// </summary>
    public class UnitOfWorkNoDb : IUnitOfWork, IDisposable
    {
        #region Private Fields

        private bool _isDisposed = false;
        private Dictionary<Type, object> _repositories;

        #endregion Private Fields

        #region Public Constructors

        public UnitOfWorkNoDb()
        {
            _repositories = new Dictionary<Type, object>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetRepository<T>(IRepository<T> repository) where T : class
        {
            _repositories[typeof(T)] = repository;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            IRepository<T> repository = null;

            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                throw new InvalidOperationException($"Repository for type {type} must be added via SetRepository() method.");
            }
            else
            {
                repository = _repositories[type] as IRepository<T>;
            }

            return repository;
        }

        public int SaveChanges()
        {
            return 0;
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // No disposable objects yet.
                }

                _isDisposed = true;
            }
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetRepository<T>().Entities.Where(predicate);
        }

        /// <summary>
        /// Return a queryable collection of entities.
        /// </summary>
        public IQueryable<T> GetEntities<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            return GetRepository<T>().GetEntities(includes);
        }

        /// <summary>
        /// Returns the first entity that satisfies a condition or null if no such entity is found.
        /// </summary>
        public T FirstOrDefault<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
        {
            return GetRepository<T>().Entities.FirstOrDefault(predicate);
        }

        public void LoadReference<T, TElement>(T entity, Expression<Func<T, ICollection<TElement>>> collection)
            where T : class
            where TElement : class
        {
            // Nothing to do
        }

        public void LoadReference<T, TElement>(T entity, Expression<Func<T, TElement>> property)
            where T : class
            where TElement : class
        {
            // Nothing to do
        }

        public ICollection<TElement> GetReference<T, TElement>(T entity, Expression<Func<T, ICollection<TElement>>> collection)
            where T : class
            where TElement : class
        {
            // Compile then invoke the expression, passing it the entity.  This should return the collection.
            return collection.Compile().Invoke(entity);
        }

        public TElement GetReference<T, TElement>(T entity, Expression<Func<T, TElement>> property)
            where T : class
            where TElement : class
        {
            // Compile then invoke the expression, passing it the entity.  This should return the property value.
            return property.Compile().Invoke(entity);
        }

        public ICollection<T> GetLocal<T>() where T : class
        {
            // Just return the set of entities.
            return GetEntities<T>().ToList();
        }

        public void Attach<T>(T entity) where T : class
        {
            var entities = (ICollection<T>)(GetRepository<T>().Entities);
            if (!entities.Contains(entity))
            {
                entities.Add(entity);
            }
        }

        public T Create<T>() where T : class
        {
            return GetRepository<T>().Create();
        }

        public void Insert<T>(T entity) where T : class
        {
            ((ICollection<T>)(GetRepository<T>().Entities)).Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            ((ICollection<T>)(GetRepository<T>().Entities)).Remove(entity);
        }

        public T GetById<T>(Guid id) where T : class
        {
            return GetRepository<T>().GetById(id);
        }

        public T GetById<T>(Guid id, params Expression<Func<T, object>>[] includes) where T : class
        {
            return GetRepository<T>().GetById(id, includes);
        }

        #endregion Protected Methods
    }

}

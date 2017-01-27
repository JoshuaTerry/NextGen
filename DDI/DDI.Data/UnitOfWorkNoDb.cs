using DDI.Shared;
using DDI.Shared.Extensions;
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
    /// Unit of Work for tests - Manages a set of mocked repositories or RepositoryNoDb instances.
    /// </summary>
    public class UnitOfWorkNoDb : IUnitOfWork, IDisposable
    {
        #region Private Fields

        private bool _isDisposed = false;
        private Dictionary<Type, object> _repositories;
        private List<object> _businessLogic;

        #endregion Private Fields

        #region Public Constructors

        public UnitOfWorkNoDb()
        {
            _repositories = new Dictionary<Type, object>();
            _businessLogic = new List<object>();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Assign a mocked repository.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository"></param>
        public void SetRepository<T>(IRepository<T> repository) where T : class
        {
            _repositories[typeof(T)] = repository;
        }

        /// <summary>
        /// Create a RepositoryNoDb for a data source.
        /// </summary>
        public IRepository<T> CreateRepositoryForDataSource<T>(IQueryable<T> dataSource) where T : class
        {
            RepositoryNoDb<T> repository = null;
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                repository = new RepositoryNoDb<T>(dataSource);
                _repositories[type] = repository;
                repository.AssignForeignKeys();
            }
            else
            {
                repository = _repositories[type] as RepositoryNoDb<T>;
            }

            return repository;
        }

        /// <summary>
        /// Create a RepositoryNoDb for a data source.
        /// </summary>
        public IRepository<T> CreateRepositoryForDataSource<T>(IList<T> dataSource) where T : class
        {
            return CreateRepositoryForDataSource(dataSource.AsQueryable());
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            IRepository<T> repository = GetRepositoryOrNull<T>();

            if (repository == null)
            {             
                throw new InvalidOperationException($"Repository for type {typeof(T)} must be added via SetRepository() method.");
            }

            return repository;
        }

        public IRepository<T> GetRepositoryOrNull<T>() where T : class
        {
            IRepository<T> repository = null;

            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                return null;
            }
            else
            {
                repository = _repositories[type] as IRepository<T>;
            }

            return repository;
        }

        public IRepository<T> GetCachedRepository<T>() where T : class
        {
            return GetRepository<T>();
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

        public IQueryable GetEntities(Type type)
        {
            IRepository repository = _repositories.GetValueOrDefault(type) as IRepository;
            return repository?.Entities;
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
            if (entity != null)
            {
                GetRepository<T>().Attach(entity);
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

        public void AddBusinessLogic(object blObj)
        {
            if (!_businessLogic.Contains(blObj))
                _businessLogic.Add(blObj);
        }

        public T GetBusinessLogic<T>() where T : class
        {
            Type blType = typeof(T);
            T blObj = _businessLogic.FirstOrDefault(p => p.GetType() == blType) as T;
            if (blObj == null)
            {
                blObj = (T)Activator.CreateInstance(blType, this);
                AddBusinessLogic(blObj);
            }

            return blObj;
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

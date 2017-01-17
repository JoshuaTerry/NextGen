using DDI.Shared;
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
    /// Unit of Work for Entity Framework - Manages a set of entity repositories sharing a dbcontext.
    /// </summary>
    public class UnitOfWorkEF : IUnitOfWork, IDisposable
    {
        #region Private Fields

        private DbContext _clientContext;
        private DbContext _commonContext;
        private bool _isDisposed = false;
        private Dictionary<Type, object> _repositories;
        private string _commonNamespace;
        private List<object> _businessLogic;

        #endregion Private Fields

        #region Public Constructors

        public UnitOfWorkEF() : this(null)
        {

        }

        public UnitOfWorkEF(DbContext context)
        {
            if (context is CommonContext)
            {
                _commonContext = context;
            }
            else if (context is DomainContext)
            {
                _clientContext = context;
            }

            _repositories = new Dictionary<Type, object>();
            _commonNamespace = typeof(Shared.Models.Common.Country).Namespace;            
            _businessLogic = new List<object>();
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

        /// <summary>
        /// Return a queryable collection of entities filtered by a predicate.
        /// </summary>
        public IQueryable<T> Where<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
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
        /// Return a queryable collection of entities.
        /// </summary>
        public IQueryable GetEntities(Type type)
        {
            return GetContext(type).Set(type);
        }

        /// <summary>
        /// Returns the first entity that satisfies a condition or null if no such entity is found.
        /// </summary>
        public T FirstOrDefault<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
        {
            return GetRepository<T>().Entities.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Explicitly load a reference property or collection for an entity.
        /// </summary>
        public void LoadReference<T, TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class where T : class
        {
            GetRepository<T>().LoadReference(entity, collection);
        }

        /// <summary>
        /// Explicitly load a reference property or collection for an entity.
        /// </summary>
        public void LoadReference<T, TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class where T : class
        {
            GetRepository<T>().LoadReference(entity, property);
        }

        /// <summary>
        /// Explicitly load a reference property or collection for an entity and return the value.
        /// </summary>
        public ICollection<TElement> GetReference<T, TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class where T : class
        {
            return GetRepository<T>().GetReference<TElement>(entity, collection);
        }

        /// <summary>
        /// Explicitly load a reference property or collection for an entity and return the value.
        /// </summary>
        public TElement GetReference<T, TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class where T : class
        {
            return GetRepository<T>().GetReference<TElement>(entity, property);
        }

        /// <summary>
        /// Return a collection of entities that have already been loaded or added to the repository.
        /// </summary>
        public ICollection<T> GetLocal<T>() where T : class
        {
            return GetRepository<T>().GetLocal();
        }

        /// <summary>
        /// Attach an entity (which may belong to another context) to the unit of work.
        /// </summary>
        public void Attach<T>(T entity) where T : class
        {
            GetRepository<T>().Attach(entity);
        }

        public T Create<T>() where T : class
        {
            return GetRepository<T>().Create();
        }

        public void Insert<T>(T entity) where T : class
        {
            GetRepository<T>().Insert(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            GetRepository<T>().Delete(entity);
        }

        public T GetById<T>(Guid id) where T : class
        {
            return GetRepository<T>().GetById(id);
        }

        public T GetById<T>(Guid id, params Expression<Func<T, object>>[] includes) where T : class
        {
            return GetRepository<T>().GetById(id, includes);
        }
        
        public IRepository<T> GetRepository<T>() where T : class
        {
            IRepository<T> repository = null;

            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                DbContext context = GetContext(type);

                // Create a repository, then add it to the dictionary.
                repository = new Repository<T>(context);

                _repositories.Add(type, repository);
            }
            else
            {
                // Repository already exists...
                repository = _repositories[type] as IRepository<T>;
            }

            return repository;
        }

        public IRepository<T> GetCachedRepository<T>() where T : class 
        {
            IRepository<T> repository = null;

            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                DbContext context = GetContext(type);

                // Create a repository, then add it to the dictionary.
                repository = new CachedRepository<T>(context);

                _repositories.Add(type, repository);
            }
            else
            {
                // Repository already exists...
                repository = _repositories[type] as IRepository<T>;
            }

            return repository;
        }

        /// <summary>
        /// Get (create if necessary) the correct DbContext for a given entity type.
        /// </summary>
        private DbContext GetContext(Type type)
        {
            DbContext context = null;

            // Get or create the appropriate context for the type.
            if (type.Namespace == _commonNamespace)
            {
                // Common context
                if (_commonContext == null)
                {
                    _commonContext = new CommonContext();
                }
                context = _commonContext;
            }
            else
            {
                // Client context
                if (_clientContext == null)
                {
                    _clientContext = new DomainContext();
                }
                context = _clientContext;
            }
            return context;
        }

        /// <summary>
        /// Saves all changes made to the unit of work to the database.
        /// </summary>
        public int SaveChanges()
        {
            return (_clientContext?.SaveChanges() ?? 0) +
                   (_commonContext?.SaveChanges() ?? 0);
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

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _clientContext?.Dispose();
                    _commonContext?.Dispose();
                }

                _isDisposed = true;
            }
        }

        #endregion Protected Methods
    }

}

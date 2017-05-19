using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DDI.EFAudit;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Data;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.Security;

namespace DDI.Data
{
    /// <summary>
    /// Unit of Work for Entity Framework - Manages a set of entity repositories sharing a dbcontext.
    /// </summary>
    public class UnitOfWorkEF : IUnitOfWork, IDisposable
    {
        #region Private Fields
        
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(UnitOfWorkEF));
        private DomainContext _clientContext;
        private DbContext _commonContext;
        private bool _isDisposed = false;
        private Dictionary<Type, object> _repositories;
        private Dictionary<Type, object> _cachedRepositories;
        private string _commonNamespace;
        private List<object> _businessLogic;
        private bool _isAuditStatusInitialized;
        private bool _auditModuleEnabled;
        private bool _auditingEnabled;
        private DbContextTransaction _dbTransaction;
        private static object _lockObject;

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
                _clientContext = (DomainContext)context;
            }

            _repositories = new Dictionary<Type, object>();
            _cachedRepositories = null;
            _commonNamespace = typeof(Shared.Models.Common.Country).Namespace;
            _businessLogic = new List<object>();
            _isAuditStatusInitialized = false;
            _dbTransaction = null;
        }

        static UnitOfWorkEF()
        {
            _lockObject = new object();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Returns TRUE if the audit module is enabled.  Can be set to FALSE to disable auditing.
        /// </summary>
        public bool AuditingEnabled
        {
            get
            {
                if (!_isAuditStatusInitialized)
                {
                    InitializeAuditStatus();
                }
                return _auditingEnabled;
            }
            set
            {
                if (!_isAuditStatusInitialized)
                {
                    InitializeAuditStatus();
                }
                _auditingEnabled = _auditModuleEnabled && value;
            }
        }

        #endregion

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
        /// Determine if any entries exist in a collection of entities filtered by a predicate.
        /// </summary>
        public bool Any<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : class
        {
            return GetRepository<T>().Entities.Any(predicate);
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
            try
            {
                return GetRepository<T>().GetReference<TElement>(entity, property);
            }
            catch
            {
                _logger.LogError($"GetReference on type {typeof(T).Name} failed for {property.Name}.");
                return null;
            }
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
        public T Attach<T>(T entity) where T : class
        {
            if (entity != null)
            {
                return GetRepository<T>().Attach(entity);
            }
            return null;
        }

        public T Create<T>() where T : class
        {
            return GetRepository<T>().Create();
        }

        public void Insert<T>(T entity) where T : class
        {
            GetRepository<T>().Insert(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            GetRepository<T>().Update(entity);
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

        /// <summary>
        /// Get a cached repository for an entity type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRepository<T> GetCachedRepository<T>() where T : class
        {
            IRepository<T> repository = null;

            var type = typeof(T);
            if (_cachedRepositories == null)
            {
                _cachedRepositories = new Dictionary<Type, object>();
            }

            if (!_cachedRepositories.ContainsKey(type))
            {
                DbContext context = GetContext(type);

                // Create a repository, then add it to the dictionary.
                repository = new CachedRepository<T>(GetRepository<T>());

                _cachedRepositories.Add(type, repository);
            }
            else
            {
                // Repository already exists...
                repository = _cachedRepositories[type] as IRepository<T>;
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
            User user;

            if (AuditingEnabled && (user = UserHelper.GetCurrentUser(this)) != null)
            {
                return (_clientContext?.Save(user).AffectedObjectCount ?? 0) +
                       (_commonContext?.SaveChanges() ?? 0);
            }
            else
            {
                return (_clientContext?.SaveChanges() ?? 0) +
                       (_commonContext?.SaveChanges() ?? 0);
            }
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            lock (_lockObject)
            {
                if (_dbTransaction != null)
                {
                    throw new InvalidOperationException("Cannot begin a new transcation because a transaction has already been started.");
                }
                if (_clientContext == null)
                {
                    _clientContext = new DomainContext();
                }
                _dbTransaction = _clientContext.Database.BeginTransaction(isolationLevel);                
            }
        }

        public void RollbackTransaction()
        {
            lock(_lockObject)
            {
                _dbTransaction?.Rollback();
                _dbTransaction?.Dispose();
                _dbTransaction = null;
            }
        }

        public bool CommitTransaction()
        {
            bool success = true;

            lock(_lockObject)
            {
                try
                {
                    SaveChanges();
                    _dbTransaction?.Commit();
                    _dbTransaction?.Dispose();
                    _dbTransaction = null;
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException)
                {
                    _dbTransaction = _clientContext.Database.CurrentTransaction;
                    success = false;
                }
            }
            return success;
        }

        public void AddBusinessLogic(object logic)
        {
            if (!_businessLogic.Contains(logic))
                _businessLogic.Add(logic);
        }

        /// <summary>
        /// Get (or create) a business logic instance associated with this unit of work.
        /// </summary>
        /// <typeparam name="T">Business logic type</typeparam>
        public T GetBusinessLogic<T>() where T : class
        {
            return GetBusinessLogic(typeof(T)) as T;
        }

        /// <summary>
        /// Get (or create) a business logic instance associated with this unit of work.
        /// </summary>
        /// <param name="logicType">Business logic type</param>
        public object GetBusinessLogic(Type logicType)
        {
            object logic = _businessLogic.FirstOrDefault(p => p.GetType() == logicType);
            if (logic == null)
            {
                logic = Activator.CreateInstance(logicType, this);
                AddBusinessLogic(logic);
            }
            return logic;
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

        public IList<T> GetRepositoryDataSource<T>() where T : class
        {
            return GetEntities<T>().ToList();
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _dbTransaction?.Rollback();
                    _dbTransaction?.Dispose();
                    _clientContext?.Dispose();
                    _commonContext?.Dispose();
                }

                _isDisposed = true;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Determine if the audit module is enabled.
        /// </summary>
        private void InitializeAuditStatus()
        {
            _auditingEnabled = _auditModuleEnabled = EFAuditModule.IsAuditEnabled;
            _isAuditStatusInitialized = true;
        }

        #endregion
    }

}

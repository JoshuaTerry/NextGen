using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public int SaveChanges()
        {
            return (_clientContext?.SaveChanges() ?? 0) +
                   (_commonContext?.SaveChanges() ?? 0);
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

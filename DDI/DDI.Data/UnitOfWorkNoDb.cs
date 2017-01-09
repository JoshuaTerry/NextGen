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

        #endregion Protected Methods
    }

}

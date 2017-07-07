using DDI.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Shared.Data
{
    public class RepositoryFactoryNoDb : IRepositoryFactory
    {
        private IList<IDisposable> _objectsToDispose;

        public RepositoryFactoryNoDb()
        {
            _objectsToDispose = new List<IDisposable>();
        }

        public IRepository<T> CreateRepository<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public IRepository<T> CreateRepository<T>(IQueryable<T> dataSource) where T : class
        {
            return new RepositoryNoDb<T>(dataSource);
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var uow = new UnitOfWorkNoDb();
            _objectsToDispose.Add(uow);
            return uow;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _objectsToDispose.ForEach(p => p.Dispose());
                }
                _objectsToDispose.Clear();
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion

    }
}

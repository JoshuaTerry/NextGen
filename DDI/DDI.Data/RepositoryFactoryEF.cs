using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Data;
using DDI.Shared.Extensions;
using DDI.Shared.Interfaces;

namespace DDI.Data
{
    public class RepositoryFactoryEF : IRepositoryFactory
    {
        private IList<IDisposable> _objectsToDispose;

        public RepositoryFactoryEF()
        {
            _objectsToDispose = new List<IDisposable>();
        }

        public IRepository<T> CreateRepository<T>() where T : class
        {
            return new Repository<T>();
        }

        public IRepository<T> CreateRepository<T>(IQueryable<T> dataSource) where T : class
        {
            return new RepositoryNoDb<T>(dataSource);
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            IUnitOfWork uow = new UnitOfWorkEF();
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

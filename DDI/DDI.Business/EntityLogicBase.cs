using System;
using DDI.Shared;
using DDI.Shared.Models;

namespace DDI.Business
{
    public class EntityLogicBase<T> : IDisposable where T : class, IEntity
    {

        #region Constructors 

        public EntityLogicBase(IUnitOfWork uow)
        {
            this.UnitOfWork = uow;
            uow.AddBusinessLogic(this);
        }

        #endregion

        #region Public Properties

        public IUnitOfWork UnitOfWork { get; private set; }

        #endregion

        #region Public Methods

        public virtual void Validate(T entity) { }

        public virtual Shared.Models.Search.ISearchDocument BuildSearchDocument(T entity)
        {
            return null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    UnitOfWork?.Dispose();
                }

                UnitOfWork = null;

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

        #endregion

    }
}

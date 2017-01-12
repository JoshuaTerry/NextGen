using DDI.Shared;
using DDI.Shared.Models;

namespace DDI.Business
{
    public class EntityLogicBase<T>  where T : class, IEntity
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

        #endregion

    }
}

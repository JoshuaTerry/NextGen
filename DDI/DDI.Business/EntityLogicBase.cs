using System.Collections;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models;

namespace DDI.Business
{
    /// <summary>
    /// Strongly typed base class for entity business logic.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public class EntityLogicBase<T> : EntityLogicBase where T : class, IEntity
    {

        #region Constructors 

        public EntityLogicBase(IUnitOfWork uow) : base(uow) { }
    
        #endregion

        #region Public Properties


        #endregion

        #region Public Methods

        /// <summary>
        /// Validate an entity.
        /// </summary>
        public virtual void Validate(T entity) { }

        /// <summary>
        /// Validate an entity.
        /// </summary>
        public override void Validate(IEntity entity)
        {
            T typedEntity = entity as T;
            if (typedEntity != null)
            {
                Validate(typedEntity);
            }
        }
        #endregion

    }

    /// <summary>
    /// Non-generic, non-strongly-typed base class for entity business logic.
    /// </summary>
    public class EntityLogicBase : IEntityLogic
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public EntityLogicBase(IUnitOfWork uow)
        {
            this.UnitOfWork = uow;
            uow.AddBusinessLogic(this);            
        }

        /// <summary>
        /// Validate an entity.
        /// </summary>
        public virtual void Validate(IEntity entity) { }

    }
}

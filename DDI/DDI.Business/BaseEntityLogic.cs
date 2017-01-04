using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Data.Models;

namespace DDI.Business
{
    public class BaseEntityLogic<T>  where T : class, IEntity
    {

        #region Constructors 

        public BaseEntityLogic(IUnitOfWork uow)
        {
            this.UnitOfWork = uow;
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

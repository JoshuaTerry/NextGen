using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    class BusinessUnitLogic : EntityLogicBase<BusinessUnit>
    {
      
        #region Constructors 

        public BusinessUnitLogic() : this(new UnitOfWorkEF()) { }

        public BusinessUnitLogic(IUnitOfWork uow) : base(uow)
        {
           
        }

        #endregion

        #region Validate Logic


        public override void Validate(BusinessUnit unit)
        {
            base.Validate(unit);

            ValidateBusinessUnitCode(unit);

            if (string.IsNullOrWhiteSpace(unit.Name))
            {
                throw new ValidationException(UserMessagesGL.NameIsRequired);
            }
        }

        private void ValidateBusinessUnitCode(BusinessUnit unit)
        {

            if (string.IsNullOrWhiteSpace(unit.Code))
            {
                throw new ValidationException(UserMessagesGL.CodeIsRequired);
            }

            if (unit.Code.Length > 8)
            {
                throw new ValidationException(UserMessagesGL.CodeMaxLengthError);
            }

            if (!Regex.IsMatch(unit.Code, @"(^[a-zA-Z0-9]+$)"))
            {
                throw new ValidationException(UserMessagesGL.CodeAlphaNumericRequired);
            }

            var existing = UnitOfWork.FirstOrDefault<BusinessUnit>(bu => bu.Code == unit.Code && bu.Id != unit.Id);
            if (existing != null)

            {
                throw new ValidationException(UserMessagesGL.CodeIsNotUnique);
            }

            if (string.IsNullOrWhiteSpace(unit.Name))
            {
                throw new ValidationException(UserMessagesGL.NameIsRequired);
            }

            //Logic to check for changing the BusinessUnitType on an edit. this needs to change once where validations are called changes
            var repository = UnitOfWork.GetRepository<BusinessUnit>();
            if (repository.GetEntityState(unit) == EntityState.Modified &&
                UnitOfWork.GetRepository<BusinessUnit>().GetModifiedProperties(unit).Contains(nameof(BusinessUnit.BusinessUnitType)))
            {
                throw new ValidationException(UserMessagesGL.BusinessUnitTypeNotEditable);
            }
        }
        #endregion
    }
}

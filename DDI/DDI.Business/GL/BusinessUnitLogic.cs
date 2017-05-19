using System.Text.RegularExpressions;
using DDI.Shared;
using DDI.Shared.Caching;
using DDI.Shared.Enums.GL;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;
using DDI.Shared.Statics;
using DDI.Shared.Statics.GL;

namespace DDI.Business.GL
{
    public class BusinessUnitLogic : EntityLogicBase<BusinessUnit>
    {
        public static string IsMultipleCacheKey => "IsMultipleBusinessUnits";

        public BusinessUnitLogic() : this(Factory.CreateUnitOfWork())
        {
        }

        public BusinessUnitLogic(IUnitOfWork uow) : base(uow)
        {
        }

        #region Validate Logic

        /// <summary>
        /// Returns TRUE if multiple business units have been defined.
        /// </summary>
        public bool IsMultiple
        {
            get
            {
                object result = CacheHelper.GetEntry<object>(IsMultipleCacheKey, () => GetIsMultiple());
                if (result is bool)
                {
                    return (bool)result;
                }
                return false;
            }
        }

        private object GetIsMultiple()
        {
            return UnitOfWork.FirstOrDefault<BusinessUnit>(p => p.BusinessUnitType != BusinessUnitType.Organization) != null;
        }

        /// <summary>
        /// Return the default business unit for the currently logged in user.
        /// </summary>
        /// <returns></returns>
        public BusinessUnit GetDefaultBusinessUnit()
        {
            if (IsMultiple)
            {
                User user = UserHelper.GetCurrentUser(UnitOfWork);
                return UnitOfWork.GetReference(user, p => p.DefaultBusinessUnit);
            }

            return UnitOfWork.FirstOrDefault<BusinessUnit>(p => p.BusinessUnitType == BusinessUnitType.Organization);
        }

        public override void Validate(BusinessUnit unit)
        {
            base.Validate(unit);

            ValidateBusinessUnitCode(unit);

            if (string.IsNullOrWhiteSpace(unit.Name))
            {
                throw new ValidationException(UserMessages.NameIsRequired);
            }
        }

        private void ValidateBusinessUnitCode(BusinessUnit unit)
        {

            if (string.IsNullOrWhiteSpace(unit.Code))
            {
                throw new ValidationException(string.Format(UserMessages.CodeIsRequired, "Business unit"));
            }

            if (unit.Code.Length > 8)
            {
                throw new ValidationException(string.Format(UserMessages.CodeMaxLengthError, "Business unit", 8));
            }

            if (!Regex.IsMatch(unit.Code, @"(^[a-zA-Z0-9]+$)"))
            {
                throw new ValidationException(string.Format(UserMessages.CodeAlphaNumericRequired, "Business unit"));
            }


            var existing = UnitOfWork.FirstOrDefault<BusinessUnit>(bu => bu.Code == unit.Code && bu.Id != unit.Id);
            if (existing != null)
            {
                throw new ValidationException(string.Format(UserMessages.CodeIsNotUnique, "Business unit"));
            }

            if (string.IsNullOrWhiteSpace(unit.Name))
            {
                throw new ValidationException(string.Format(UserMessages.NameIsRequired, "Business unit"));
            }

            //Logic to check for changing the BusinessUnitType on an edit. this needs to change once where validations are called changes
            var repository = UnitOfWork.GetRepository<BusinessUnit>();
            if (repository.GetEntityState(unit) == EntityState.Modified && 
                UnitOfWork.GetRepository<BusinessUnit>().GetModifiedProperties(unit).Contains(nameof(BusinessUnit.BusinessUnitType))) { 
                    throw new ValidationException(UserMessagesGL.BusinessUnitTypeNotEditable);
            }

            CacheHelper.RemoveEntry(IsMultipleCacheKey);

        }
        #endregion
    }
}

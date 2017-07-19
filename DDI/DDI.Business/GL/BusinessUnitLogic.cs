using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DDI.Business.Core;
using DDI.Business.Helpers;
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
        public static string BusinessUnitLabelCacheKey => "BusinessUnitLabel";

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

        /// <summary>
        /// Returns the client-specific business unit label.
        /// </summary>
        public string BusinessUnitLabel
        {
            get
            {
                string label = CacheHelper.GetEntry(BusinessUnitLabelCacheKey, () => 
                    UnitOfWork.GetBusinessLogic<ConfigurationLogic>().GetConfiguration<CoreConfiguration>().BusinessUnitLabel);
                if (string.IsNullOrWhiteSpace(label))
                {
                    return "Business Unit";                    
                }
                return label;
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
                if (user?.DefaultBusinessUnitId != null)
                {
                    return UnitOfWork.GetById<BusinessUnit>(user.DefaultBusinessUnitId.Value);
                }
                else
                {
                    return null;
                }
            }

            return UnitOfWork.FirstOrDefault<BusinessUnit>(p => p.BusinessUnitType == BusinessUnitType.Organization);
        }

        public override void Validate(BusinessUnit unit)
        {
            base.Validate(unit);

            if (string.IsNullOrWhiteSpace(unit.Name))
            {
                throw new ValidationException(UserMessages.NameIsRequired);
            }        

            if (string.IsNullOrWhiteSpace(unit.Code))
            {
                throw new ValidationException(UserMessages.CodeIsRequired, BusinessUnitLabel);
            }

            if (unit.Code.Length > 8)
            {
                throw new ValidationException(UserMessages.CodeMaxLengthError, BusinessUnitLabel, "8");
            }

            if (!Regex.IsMatch(unit.Code, @"(^[a-zA-Z0-9]+$)"))
            {
                throw new ValidationException(UserMessages.CodeAlphaNumericRequired, BusinessUnitLabel);
            }

            var existing = UnitOfWork.FirstOrDefault<BusinessUnit>(bu => bu.Code == unit.Code && bu.Id != unit.Id);
            if (existing != null)
            {
                throw new ValidationException(UserMessages.CodeIsNotUnique, BusinessUnitLabel);
            }

            if (string.IsNullOrWhiteSpace(unit.Name))
            {
                throw new ValidationException(UserMessages.NameIsRequired, BusinessUnitLabel);
            }

            //Logic to check for changing the BusinessUnitType on an edit. this needs to change once where validations are called changes
            var repository = UnitOfWork.GetRepository<BusinessUnit>();
            var entityState = repository.GetEntityState(unit);
            bool isNewUnit = false;
            bool isModified = false;
            List<string> modifiedProperites = null;

            if (entityState != EntityState.Added)
            {
                modifiedProperites = repository.GetModifiedProperties(unit);
                isModified = modifiedProperites.Count > 0;
            }
            else
            {
                isNewUnit = true;
            }

            if (isModified && modifiedProperites.Contains(nameof(BusinessUnit.BusinessUnitType)))
            { 
                throw new ValidationException(UserMessagesGL.BusinessUnitTypeNotEditable);
            }

            var ledgerLogic = UnitOfWork.GetBusinessLogic<LedgerLogic>();

            if (isModified && (modifiedProperites.Contains(nameof(BusinessUnit.Code)) || modifiedProperites.Contains(nameof(BusinessUnit.Name))))
            {
                UpdateLedgerCodeAndName(unit, repository.GetOriginalPropertyValue(unit, p => p.Code), repository.GetOriginalPropertyValue(unit, p => p.Name));
            }
            else if (isNewUnit)
            {
                CreateLedgerForBusinessUnit(unit);
                LinkUserToBusinessUnit(unit);
            }
            
            CacheHelper.RemoveEntry(IsMultipleCacheKey);

            if (isModified || isNewUnit)
            {
                ledgerLogic.InvalidateLedgerCache();
            }

        }

        private void LinkUserToBusinessUnit(BusinessUnit unit)
        {
            if (unit.Users == null)
            {
                unit.Users = new List<User>();
            }
            User currentUser = UserHelper.GetCurrentUser(UnitOfWork, true);
            if (!unit.Users.Contains(currentUser))
            {
                unit.Users.Add(currentUser);
            }
        }

        private void CreateLedgerForBusinessUnit(BusinessUnit unit)
        {
            Ledger fromLedger = null;
            Ledger ledger = new Ledger()
            {
                Name = unit.Name,
                Code = unit.Code,
                Status = LedgerStatus.Empty,
                BusinessUnit = unit

            };

            if (unit.BusinessUnitType == BusinessUnitType.Common)
            {
                fromLedger = UnitOfWork.GetEntities<Ledger>(p => p.SegmentLevels).FirstOrDefault(p => p.BusinessUnit.BusinessUnitType == BusinessUnitType.Organization);
                ledger.OrgLedger = fromLedger;
            }

            var ledgerLogic = UnitOfWork.GetBusinessLogic<LedgerLogic>();

            if (fromLedger != null)
            {
                ledgerLogic.CopyLedgerProperties(fromLedger, ledger);
                ledgerLogic.CopySegmentLevels(fromLedger, ledger);
            }
            else
            {
                ledgerLogic.InitializeLedgerProperties(ledger);
            }

            UnitOfWork.Insert(ledger);
        }


        private void UpdateLedgerCodeAndName(BusinessUnit unit, string originalCode, string originalName)
        {
            foreach (var ledger in UnitOfWork.Where<Ledger>(p => p.BusinessUnitId == unit.Id))
            {
                if (string.IsNullOrWhiteSpace(ledger.Code) || StringHelper.IsSameAs(originalCode, ledger.Code))
                {
                    ledger.Code = unit.Code;
                }
                if (string.IsNullOrWhiteSpace(ledger.Name) || StringHelper.IsSameAs(originalName, ledger.Name))
                {
                    ledger.Name = unit.Name;
                }
            }
        }


        #endregion
    }
}

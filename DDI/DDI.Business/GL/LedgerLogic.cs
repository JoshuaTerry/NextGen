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
    class LedgerLogic : EntityLogicBase<Ledger>
    {
      
        #region Constructors 

        public LedgerLogic() : this(new UnitOfWorkEF()) { }

        public LedgerLogic(IUnitOfWork uow) : base(uow)
        {
           
        }

        #endregion

        #region Validate Logic


        public override void Validate(Ledger unit)
        {
            base.Validate(unit);

            ValidateLedgerNames(unit);

            ValidateLedgerGroupLevels(unit);

        }

        private void ValidateLedgerNames(Ledger unit)
        {
            switch (unit.AccountGroupLevels)
            {

                case 1:
                    if (string.IsNullOrWhiteSpace(unit.AccountGroup1Title))
                    {
                        throw new ValidationException(UserMessagesGL.BlankGroupName);
                    }
                    break;

                case 2:
                    if (string.IsNullOrWhiteSpace(unit.AccountGroup1Title) || string.IsNullOrWhiteSpace(unit.AccountGroup2Title))
                    {
                        throw new ValidationException(UserMessagesGL.BlankGroupName);
                    }
                    if (unit.AccountGroup1Title == unit.AccountGroup2Title)
                    {
                        throw new ValidationException(UserMessagesGL.DuplicateGroupName);
                    }
                    break;

                case 3:
                    if (string.IsNullOrWhiteSpace(unit.AccountGroup1Title) || string.IsNullOrWhiteSpace(unit.AccountGroup2Title)
                        || string.IsNullOrWhiteSpace(unit.AccountGroup3Title))
                    {
                        throw new ValidationException(UserMessagesGL.BlankGroupName);
                    }
                    if (unit.AccountGroup1Title == unit.AccountGroup2Title || unit.AccountGroup1Title == unit.AccountGroup3Title
                        || unit.AccountGroup2Title == unit.AccountGroup3Title)
                    {
                        throw new ValidationException(UserMessagesGL.DuplicateGroupName);
                    }
                    break;

                case 4:
                    if (string.IsNullOrWhiteSpace(unit.AccountGroup1Title) || string.IsNullOrWhiteSpace(unit.AccountGroup2Title)
                        || string.IsNullOrWhiteSpace(unit.AccountGroup3Title) || string.IsNullOrWhiteSpace(unit.AccountGroup4Title))
                    {
                        throw new ValidationException(UserMessagesGL.BlankGroupName);
                    }
                    if (unit.AccountGroup1Title == unit.AccountGroup2Title || unit.AccountGroup1Title == unit.AccountGroup3Title
                        || unit.AccountGroup1Title == unit.AccountGroup4Title || unit.AccountGroup2Title == unit.AccountGroup3Title
                        || unit.AccountGroup2Title == unit.AccountGroup4Title || unit.AccountGroup3Title == unit.AccountGroup4Title)
                    {
                        throw new ValidationException(UserMessagesGL.DuplicateGroupName);
                    }
                    break;

            }

        }

        private void ValidateLedgerGroupLevels(Ledger unit)
        {
            //Logic to check for changing the LedgerType on an edit. this needs to change once where validations are called changes
            //var repository = UnitOfWork.GetRepository<Ledger>();
            //if (repository.GetEntityState(unit) == EntityState.Modified &&
            //    UnitOfWork.GetRepository<Ledger>().GetModifiedProperties(unit).Contains(nameof(Ledger.LedgerType)))
            //{
            //    throw new ValidationException(UserMessagesGL.LedgerTypeNotEditable);
            //}
        }
        #endregion
    }
}

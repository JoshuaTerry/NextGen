using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;

namespace DDI.Business.GL
{
    public class AccountLogic : EntityLogicBase<Account>
    {
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(AccountLogic));
        public AccountLogic() : this(new UnitOfWorkEF()) { }

        public AccountLogic(IUnitOfWork uow) : base(uow)
        {
        }

        public override void Validate(Account entity)
        {
            base.Validate(entity);

        }

        /// <summary>
        /// Get the default LedgerAccountYear for an account.
        /// </summary>
        public LedgerAccountYear GetLedgerAccountYear(Account account)
        {
            return UnitOfWork.GetReference(account, p => p.LedgerAccountYears)?.FirstOrDefault(p => p.IsMerge == false) ?? account.LedgerAccountYears.FirstOrDefault();
        }

        /// <summary>
        /// Get the default LedgerAccountYear for a LedgerAccount based on a fiscal year
        /// </summary>
        public LedgerAccountYear GetLedgerAccountYear(LedgerAccount account, FiscalYear year)
        {
            return GetLedgerAccountYear(account, year?.Id);
        }

        /// <summary>
        /// Get the default LedgerAccountYear for a LedgerAccount based on a fiscal year
        /// </summary>
        public LedgerAccountYear GetLedgerAccountYear(LedgerAccount account, Guid? yearId)
        {
            if (account == null || yearId == null)
            {
                return null;
            }

            if (account.LedgerAccountYears == null)
            {
                UnitOfWork.LoadReference(account, p => p.LedgerAccountYears);
            }

            return account.LedgerAccountYears.FirstOrDefault(p => p.FiscalYearId == yearId && p.IsMerge == false) 
                    ??
                   account.LedgerAccountYears.FirstOrDefault(p => p.FiscalYearId == yearId);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Business.GL;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.GL;

namespace DDI.Services.GL
{
    public class AccountService : ServiceBase<Account>, IAccountService
    {
        public IDataResponse<AccountActivitySummary> GetAccountActivity(Guid accountId)
        {
            Account account = UnitOfWork.GetById<Account>(accountId, p => p.FiscalYear.Ledger, p => p.Budgets);

            var activity = UnitOfWork.GetBusinessLogic<AccountLogic>().GetAccountActivity(account);
            return new DataResponse<AccountActivitySummary>(activity);
        }

        public IDataResponse<List<AccountActivityDetail>> GetAccountActivityDetail(Guid accountId)
        {
            Account account = UnitOfWork.GetById<Account>(accountId, p => p.FiscalYear.Ledger, p => p.Budgets);

            var activity = UnitOfWork.GetBusinessLogic<AccountLogic>().GetAccountActivity(account);
            List<AccountActivityDetail> activityDetailList = activity.Detail.ToList();

            return new DataResponse<List<AccountActivityDetail>>(activityDetailList);
        }

        protected override Action<Account> FormatEntityForGet => account => PopulateAccountBalanceIds(account);

        /// <summary>
        /// The Ids coming back from the SQL view put the account ID into AccountBalance.Id.  This confuses the dynamic transmogrifier, 
        /// so we need to store actual Guids instead.  These are never used, since the rows in AccountBalance can't be updated or referenced individually.
        /// </summary>
        private void PopulateAccountBalanceIds(Account account)
        {
            if (account.AccountBalances != null)
            {
                foreach (var entry in account.AccountBalances)
                {
                    entry.Id = GuidHelper.NewSequentialGuid();
                }
            }
        }


    }
}

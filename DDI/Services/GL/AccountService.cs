using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.GL;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using WebGrease.Css.Extensions;

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
            var activityDetailList = new List<AccountActivityDetail>();

            foreach(var detail in activity.Detail)
            {
                activityDetailList.Add(detail);
            }
            return new DataResponse<List<AccountActivityDetail>>(activityDetailList);
        }
    }
}

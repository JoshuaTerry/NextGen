using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.ServiceInterfaces
{
    public interface IAccountService : IService<Account>
    {
        IDataResponse<AccountActivitySummary> GetAccountActivity(Guid accountId);
        IDataResponse<List<AccountActivityDetail>> GetAccountActivityDetail(Guid id);

        IDataResponse<Account> Merge(Guid sourceAccountId, Guid destinationAccountId);
    }
}

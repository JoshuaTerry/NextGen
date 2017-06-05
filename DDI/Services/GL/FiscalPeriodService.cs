using DDI.Shared.Models.Client.GL;
using DDI.Services.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared;
using DDI.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace DDI.Services.GL
{
    public class FiscalPeriodService : ServiceBase<FiscalPeriod>, IFiscalPeriodService
    {
        public FiscalPeriodService(IUnitOfWork uow) : base(uow)
        {
        }

        public IDataResponse<List<ICanTransmogrify>> GetFiscalPeriodsByAccountId(Guid accountId)
        {
            Account account = UnitOfWork.GetById<Account>(accountId);
            if (account != null)
            {
                return GetAllWhereExpression(fp => fp.FiscalYearId == account.FiscalYearId);
            }

            return GetErrorResponse<List<ICanTransmogrify>>("Invalid account ID.");
        }

    }
}

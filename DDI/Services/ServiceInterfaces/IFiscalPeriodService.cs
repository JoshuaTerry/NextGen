using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;

namespace DDI.Services.ServiceInterfaces
{
    public interface IFiscalPeriodService : IService<FiscalPeriod>
    {
        IDataResponse<List<ICanTransmogrify>> GetFiscalPeriodsByAccountId(Guid accountId);
    }
}
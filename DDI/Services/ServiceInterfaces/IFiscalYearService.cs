using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;

namespace DDI.Services.ServiceInterfaces
{
    public interface IFiscalYearService : IService<FiscalYear>
    {
        IDataResponse<FiscalYear> Post(FiscalYear entityToSave);
        IDataResponse<List<ICanTransmogrify>> GetYearForOtherBusinessUnit(Guid unitId, Guid fiscalYearId);
    }
}

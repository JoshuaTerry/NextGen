using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.ServiceInterfaces
{
    public interface IFiscalYearService : IService<FiscalYear>
    {
        IDataResponse<FiscalYear> Post(FiscalYear entityToSave);
    }
}

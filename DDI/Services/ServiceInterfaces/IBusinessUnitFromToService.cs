using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Models;

namespace DDI.Services.ServiceInterfaces
{
    public interface IBusinessUnitFromToService : IService<BusinessUnitFromTo>
    {
        IDataResponse<List<ICanTransmogrify>> GetForFiscalYear(Guid? yearId);
    }
}

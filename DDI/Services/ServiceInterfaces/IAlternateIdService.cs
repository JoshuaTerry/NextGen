using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Services.ServiceInterfaces
{
    public interface  IAlternateIdService : IService<AlternateId>
    {
        IDataResponse<List<AlternateId>> GetAlternateIdsByConstituent(Guid constituentId);
    }
}

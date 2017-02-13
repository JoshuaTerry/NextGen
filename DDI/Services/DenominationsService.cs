using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;

namespace DDI.Services
{
    public class DenominationsService : ServiceBase<Denomination>, IDenominationsService
    {
        public IDataResponse<Constituent> AddDenominationsToConstituent(Guid id, JObject denominationIds)
        {
            return null;
        }
    }
}

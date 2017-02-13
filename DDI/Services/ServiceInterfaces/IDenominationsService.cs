using System;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;

namespace DDI.Services.ServiceInterfaces
{
    public interface IDenominationsService : IService<Denomination>
    {
        IDataResponse<Constituent> AddDenominationsToConstituent(Guid id, JObject denominationIds);
    }
}
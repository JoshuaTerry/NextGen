using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using System;
using System.Collections.Generic;

namespace DDI.Services.ServiceInterfaces
{
    public interface IRegionService : IService<Region>
    {
        IDataResponse<List<Region>> GetRegionsByLevel(Guid? id, int level);
        IDataResponse<List<Region>> GetRegionsByAddress(Guid? countryid, Guid? StateId, Guid? CountyId, string city, string zipcode);
    }
}

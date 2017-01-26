using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Services
{
    public interface IRegionService
    {
        IDataResponse<List<Region>> GetByLevel(int level, Guid? id);
    }
}

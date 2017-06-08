using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using System;
using System.Collections.Generic;

namespace DDI.Services
{
    public interface ICustomFieldService : IService<CustomField>
    {
        IDataResponse<List<CustomField>> GetByEntityId(int entityId, Guid constituentId);
    }
}

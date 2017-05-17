using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using System.Collections.Generic;

namespace DDI.Services
{
    public interface ICustomFieldService
    {
        IDataResponse<List<CustomField>> GetByEntityId(int entityId, Guid constituentId);
    }
}

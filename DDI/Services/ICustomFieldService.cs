using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;

namespace DDI.Services
{
    public interface ICustomFieldService
    {
        IDataResponse<List<CustomField>> GetByEntityId(int entityId);
    }
}

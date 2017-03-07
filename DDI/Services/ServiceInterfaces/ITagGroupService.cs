using System;
using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Services.ServiceInterfaces
{
    public interface ITagGroupService
    {
        IDataResponse<List<TagGroup>> GetByConstituentCategory(ConstituentCategory category);
    }
}

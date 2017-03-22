using DDI.Shared;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;
using System.Collections.Generic;

namespace DDI.Services.ServiceInterfaces
{
    public interface ITagGroupService
    {
        IDataResponse<List<TagGroup>> GetByConstituentCategory(ConstituentCategory category);
    }
}

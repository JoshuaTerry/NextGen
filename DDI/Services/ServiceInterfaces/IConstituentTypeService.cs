using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;
using System;

namespace DDI.Shared
{
    public interface IConstituentTypeService : IService<ConstituentType>
    {
        IDataResponse AddTagsToConstituentType(ConstituentType id, JObject tagIds);
        IDataResponse RemoveTagFromConstituentType(ConstituentType id, Guid tagId);
    }
}
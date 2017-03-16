using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;

namespace DDI.Shared
{ 
    public interface IConstituentTypeService : IService<ConstituentType>
    {
        IDataResponse AddTagsToConstituentType(ConstituentType id, JObject tagIds);
        IDataResponse RemoveTagFromConstituentType(ConstituentType id, Guid tagId);
    }
}
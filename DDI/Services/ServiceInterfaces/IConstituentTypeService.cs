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
        //IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum);
        //IDataResponse<Constituent> NewConstituent(Guid constituentTypeId);
        IDataResponse AddTagsToConstituentType(ConstituentType id, JObject tagIds);
        IDataResponse RemoveTagFromConstituentType(ConstituentType id, Guid tagId);
    }
}
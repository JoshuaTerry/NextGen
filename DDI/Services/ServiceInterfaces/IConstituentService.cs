using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;

namespace DDI.Shared
{ 
    public interface IConstituentService : IService<Constituent>
    {
        IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum);
        IDataResponse<Constituent> NewConstituent(Guid constituentTypeId);
        IDataResponse AddTagsToConstituent(Constituent id, JObject tagIds);
        IDataResponse RemoveTagFromConstituent(Constituent id, Guid tagId);
        IDataResponse Test();
    }
}
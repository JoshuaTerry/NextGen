using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;
using System;

namespace DDI.Shared
{
    public interface IConstituentService : IService<Constituent>
    {
        IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum);
        IDataResponse<Constituent> NewConstituent(Guid constituentTypeId);
        IDataResponse AddTagsToConstituent(Constituent id, JObject tagIds);
        IDataResponse RemoveTagFromConstituent(Constituent id, Guid tagId);
        IDataResponse GetConstituentPrimaryContactInfo(Constituent constituent);
    }
}
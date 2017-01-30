using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;

namespace DDI.Shared
{ 
    public interface IConstituentService
    {
        IDataResponse<List<Constituent>> GetAll(IPageable search);
        IDataResponse<Constituent> GetById(Guid id);
        IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum);
        IDataResponse<Constituent> UpdateConstituent(Guid id, JObject constituentChanges);
        IDataResponse<List<DoingBusinessAs>> GetConstituentDBAs(Guid constituentId);
        IDataResponse<List<AlternateId>> GetConstituentAlternateIds(Guid constituentId);
        IDataResponse<EducationLevel> GetEducationLevel(Guid constituentId);
        IDataResponse<Constituent> AddConstituent(Constituent constituent);
        IDataResponse<int> GetNextConstituentNumber();
        object NewConstituent(Guid id);
        IDataResponse<List<ConstituentAddress>> GetConstituentAddresses(Guid constituentId);
    }
}
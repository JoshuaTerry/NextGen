using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;

namespace DDI.Shared
{ 
    public interface IConstituentService
    {
        IDataResponse<dynamic> GetConstituents(ConstituentSearch search);
        IDataResponse<dynamic> GetConstituentById(Guid id, string fields = null);
        IDataResponse<dynamic> GetConstituentByConstituentNum(int constituentNum, string fields = null);
        IDataResponse<dynamic> UpdateConstituent(Guid id, JObject constituentChanges);
        IDataResponse<List<DoingBusinessAs>> GetConstituentDBAs(Guid constituentId);
        IDataResponse<EducationLevel> GetEducationLevels(Guid constituentId);
        IDataResponse AddConstituent(Constituent constituent);
        IDataResponse<int> GetNextConstituentNumber();
        object NewConstituent(Guid id);
    }
}
using System;
using System.Collections.Generic;
using DDI.Data.Models.Client.CRM;
using DDI.Shared;
using DDI.WebApi.Services.Search;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Services
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
        IDataResponse<Constituent> NewConstituent(Guid constituentTypeID);
    }
}
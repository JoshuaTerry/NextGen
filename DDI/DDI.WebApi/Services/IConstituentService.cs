using System;
using System.Collections.Generic;
using DDI.Data.Models.Client.CRM;
using DDI.Shared;

using Newtonsoft.Json.Linq;
using Services.Search;

namespace DDI.WebApi.Services
{
    public interface IConstituentService
    {
        IDataResponse<List<Constituent>> GetConstituents(ConstituentSearch search);
        IDataResponse<Constituent> GetConstituentById(Guid id);
        IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum);
        IDataResponse<Constituent> UpdateConstituent(Guid id, JObject constituentChanges);
        IDataResponse<List<DoingBusinessAs>> GetConstituentDBAs(Guid constituentId);
        IDataResponse<EducationLevel> GetEducationLevels(Guid constituentId);
        IDataResponse AddConstituent(Constituent constituent);
        IDataResponse<int> GetNextConstituentNumber();
    }
}
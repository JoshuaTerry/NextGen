using System;
using System.Collections.Generic; 
using DDI.Data.Models.Client;
using DDI.Shared;
using Newtonsoft.Json.Linq;

namespace DDI.Business.Services
{
    public interface IConstituentService
    {
        IDataResponse<List<Constituent>> GetConstituents(ConstituentSearch search);
        IDataResponse<Constituent> GetConstituentById(Guid id);
        IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum);
        IDataResponse<Constituent> UpdateConstituent(Guid id, JObject constituentChanges);
        IDataResponse<List<DoingBusinessAs>> GetConstituentDBAs(Guid constituentId);
        IDataResponse<EducationLevel> GetEducationLevels(Guid constituentId);
    }
}
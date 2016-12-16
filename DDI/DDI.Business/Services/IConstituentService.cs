using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DDI.Data;
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
    }
}
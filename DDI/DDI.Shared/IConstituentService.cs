﻿using System;
using System.Collections.Generic;
using DDI.Shared;
using Newtonsoft.Json.Linq;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Shared
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
        IDataResponse<Constituent> NewConstituent(Guid constituentTypeID);
    }
}
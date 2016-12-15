﻿using System;
using System.Collections.Generic; 
using DDI.Data.Models.Client;
using DDI.Shared;

namespace DDI.Business.Services
{
    public interface IConstituentService
    {
        IDataResponse<List<Constituent>> GetConstituents(ConstituentSearch search);
        IDataResponse<Constituent> GetConstituentById(Guid id);
        IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum);
    }
}
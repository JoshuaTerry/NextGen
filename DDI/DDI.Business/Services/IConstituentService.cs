using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DDI.Data;
using DDI.Data.Models.Client;
using DDI.Shared;

namespace DDI.Business.Services
{
    public interface IConstituentService
    {
        IDataResponse<IQueryable<Constituent>> GetConstituents();
        IDataResponse<Constituent> GetConstituentById(Guid id);
    }
}
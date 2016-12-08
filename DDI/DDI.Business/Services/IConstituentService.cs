using System.Collections.Generic;
using System.Web.Http;
using DDI.Data;
using DDI.Data.Models.Client;
using DDI.Shared;

namespace DDI.Business.Services
{
    public interface IConstituentService
    {
        IDataResponse<IRepository<Constituent>> GetConstituents();
        IDataResponse<IRepository<Constituent>> GetConstituentById(int id);
    }
}
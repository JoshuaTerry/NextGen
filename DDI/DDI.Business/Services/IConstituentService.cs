using System.Collections.Generic;
using System.Web.Http;
using DDI.Business.DataModels;
using DDI.Shared;

namespace DDI.Business.Services
{
    public interface IConstituentService
    {
        IDataResponse<List<Constituent>> GetConstituents();
        IDataResponse<Constituent> GetConstituent(int id);
    }
}
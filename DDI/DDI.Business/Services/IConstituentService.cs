using System.Collections.Generic;
using System.Web.Http;
using DDI.Shared;
using DDI.Shared.DTO;

namespace DDI.Business.Services
{
    public interface IConstituentService
    {
        IDataResponse<List<DtoConstituent>> GetConstituents();
        IDataResponse<DtoConstituent> GetConstituent(int id);
    }
}
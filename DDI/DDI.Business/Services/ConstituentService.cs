using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DDI.Shared;
using DDI.Shared.DTO;

namespace DDI.Business.Services
{
    public class ConstituentService : ServiceBase, IConstituentService
    {
        public IDataResponse<List<DDI.Shared.DTO.DtoConstituent>> GetConstituents()
        {
            throw new NotImplementedException();
        }

        public IDataResponse<DtoConstituent> GetConstituent(int id)
        {
            throw new NotImplementedException();
        }
    }
}
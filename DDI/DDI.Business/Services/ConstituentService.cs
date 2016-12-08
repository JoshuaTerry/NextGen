using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DDI.Business.DataModels;
using DDI.Shared;

namespace DDI.Business.Services
{
    public class ConstituentService : ServiceBase, IConstituentService
    {
        public IDataResponse<List<Constituent>> GetConstituents()
        {
            throw new NotImplementedException();
        }

        public IDataResponse<Constituent> GetConstituent(int id)
        {
            throw new NotImplementedException();
        }
    }
}
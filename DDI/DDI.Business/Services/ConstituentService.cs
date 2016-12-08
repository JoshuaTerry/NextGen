using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using DDI.Data;
using DDI.Data.Models.Client;
using DDI.Shared;

namespace DDI.Business.Services
{
    public class ConstituentService : ServiceBase, IConstituentService
    {
        private DomainContext _db = new DomainContext();

        public IDataResponse<IRepository<Constituent>> GetConstituents()
        {
            IRepository<Constituent> list = new Repository<Constituent>(_db);

            return GetIDataResponse(() => list);
        }

        public IDataResponse<IRepository<Constituent>> GetConstituentById(int id)
        {
            IRepository<Constituent> constituent = new Repository<Constituent>(_db);

            return GetIDataResponse(() => constituent);
        }
    }
}
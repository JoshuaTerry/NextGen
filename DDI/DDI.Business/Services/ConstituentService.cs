using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using DDI.Business.Domain;
using DDI.Data;
using DDI.Data.Models.Client;
using DDI.Shared;

namespace DDI.Business.Services
{
    public class ConstituentService : ServiceBase, IConstituentService
    {
        private IRepository<Constituent> _repository;

        private ConstituentDomain _domain;

        public ConstituentService():
            this(new ConstituentDomain())
        {            
        }

        internal ConstituentService(ConstituentDomain domain)
        {
            _domain = domain;
            _repository = domain.Repository; 
        }

        public IDataResponse<List<Constituent>> GetConstituents(ConstituentSearch search)
        {
            IQueryable<Constituent> constituents = _repository.Entities; 
            constituents = constituents.Where(c => (c.FirstName.Contains(search.Name) || c.LastName.Contains(search.Name)));
            constituents = constituents.OrderBy(c => c.LastName);
            var pageSize = (search.Limit ?? 100);
            if ((search.Offset ?? 0) > 0)
            {
                constituents = constituents.Skip(search.Offset.Value * pageSize);
            }
            constituents = constituents.Take(pageSize);

            return GetIDataResponse(() => constituents.ToList());
        }

        public IDataResponse<Constituent> GetConstituentById(Guid id)
        {
            Constituent constituent = _repository.GetById(id);

            return GetIDataResponse(() => constituent);
        }

        public IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum)
        {
            var constituent = _repository.Entities.FirstOrDefault(c => c.ConstituentNumber == constituentNum);

            return GetIDataResponse(() => constituent);
        }
    }
}

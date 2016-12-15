using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using DDI.Data;
using DDI.Data.Models.Client;
using DDI.Shared;
using Newtonsoft.Json.Linq;

namespace DDI.Business.Services
{
    public class ConstituentService : ServiceBase, IConstituentService
    {
        private IRepository<Constituent> _repository; 

        public ConstituentService():
            this(new Repository<Constituent>())
        {
            
        }

        internal ConstituentService(IRepository<Constituent> repository)
        {
            _repository = repository;
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
            var response = GetIDataResponse(() => constituents.ToList());
            response.Links = new List<HATEOASLink>()
            {
                new HATEOASLink()
                {
                    Href = search.ToQueryString(),
                    Relationship = "self",
                    Method = "GET"
                }
            };

            return response;
        }

        public IDataResponse<Constituent> GetConstituentById(Guid id)
        {
            Constituent constituent = _repository.GetById(id);
            var response = GetIDataResponse(() => constituent);
            response.Links= new List<HATEOASLink>()
            {
                new HATEOASLink()
                {
                    Href = $"api/v1/constituents/{id}",
                    Relationship = "self",
                    Method = "GET"
                }
            };

            return response;
        }

        public IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum)
        {
            var constituent = _repository.Entities.FirstOrDefault(c => c.ConstituentNumber == constituentNum);
            var response = GetIDataResponse(() => constituent);
            response.Links = new List<HATEOASLink>()
            {
                new HATEOASLink()
                {
                    Href = $"api/v1/constituents?constituentNum={constituentNum}",
                    Relationship = "self",
                    Method = "GET"
                }
            };

            return response;
        }

        public IDataResponse<Constituent> UpdateConstituent(Guid id, object constituentChanges)
        {
            Dictionary<string, object> changedProperties = new Dictionary<string, object>();
            var props = ((JObject)constituentChanges);

            foreach (var pair in props)
            {
                changedProperties.Add(pair.Key, pair.Value);
            }

            var constituent = _repository.GetById(id);

            return GetIDataResponse(() => constituent);
        }
    }
}
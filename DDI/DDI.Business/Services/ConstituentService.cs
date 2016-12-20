using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using DDI.Business.Domain;
using DDI.Business.Helpers;
using DDI.Business.Services.Search;
using DDI.Data;
using DDI.Data.Models.Client;
using DDI.Shared;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebGrease.Css.Extensions;

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
            IQueryable<Constituent> constituents = _repository.Entities.Include("ConstituentAddresses.Address");
            var query = new CriteriaQuery<Constituent, ConstituentSearch>(constituents, search)
                .IfModelPropertyIsNotBlankAndDatabaseContainsIt(m => m.Name, c => c.FormattedName)
                .IfModelPropertyIsNotBlankThenAndTheExpression(m => m.City, c => c.ConstituentAddresses.Any(a => a.Address.City.StartsWith(search.City)))
                .IfModelPropertyIsNotBlankThenAndTheExpression(m => m.AlternateId, c => c.AlternateIds.Any(a => a.Name.Contains(search.AlternateId)))
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.ConstituentTypeId, c => c.ConstituentTypeId)
                .SetLimit(search.Limit)
                .SetOffset(search.Offset)
                .SetOrderBy(search.OrderBy);
            // Created Range
            ApplyZipFilter(query, search);
            ApplyQuickFilter(query, search);

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            var response = GetIDataResponse(() => query.GetQueryable().ToList());
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

        private void ApplyQuickFilter(CriteriaQuery<Constituent, ConstituentSearch> query, ConstituentSearch search)
        {
            if (!search.QuickSearch.IsNullOrWhiteSpace())
            {
                query.Or(c => c.FormattedName.Contains(search.QuickSearch));
                query.Or(c => c.AlternateIds.Any(a => a.Name.Contains(search.QuickSearch)));
                int quickSearchNumber;
                if (int.TryParse(search.QuickSearch, out quickSearchNumber))
            {
                    query.Or(c => c.ConstituentNumber.Equals(quickSearchNumber));
                }
            }
            }

        private void ApplyZipFilter(CriteriaQuery<Constituent, ConstituentSearch> query, ConstituentSearch search)
        {
            if (!search.ZipFrom.IsNullOrWhiteSpace() || !search.ZipTo.IsNullOrWhiteSpace())
            {
                if (search.ZipFrom.IsNullOrWhiteSpace())
                {
                    query.And(c => c.ConstituentAddresses.Any(a => a.Address.PostalCode.StartsWith(search.ZipTo)));
                }
                else if (search.ZipTo.IsNullOrWhiteSpace())
                {
                    query.And(c => c.ConstituentAddresses.Any(a => a.Address.PostalCode.StartsWith(search.ZipFrom)));
                }
                else
                {
                    query.And(c => (c.ConstituentAddresses.Any(a => a.Address.PostalCode.CompareTo(search.ZipFrom) >= 0 && a.Address.PostalCode.CompareTo(search.ZipTo + "~") <= 0)));
                }
            }
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

        public IDataResponse<Constituent> UpdateConstituent(Guid id, JObject constituentChanges)
        {
            Dictionary<string, object> changedProperties = new Dictionary<string, object>();

            foreach (var pair in constituentChanges)
            {
                changedProperties.Add(pair.Key, pair.Value.ToObject(ConvertToType(pair.Key, new Constituent())));
            }

            _repository.UpdateChangedProperties(id, changedProperties);

            var constituent = _repository.GetById(id);

            return GetIDataResponse(() => constituent);
        }

        public IDataResponse<List<DoingBusinessAs>> GetConstituentDBAs(Guid constituentId)
        {
            Repository<DoingBusinessAs> dbaRepo = new Repository<DoingBusinessAs>();
            var data = dbaRepo.Entities.Where(d => d.ConstituentId == constituentId);

            var response = new DataResponse<List<DoingBusinessAs>> { Data = data.ToList() };
            return response;
        }

        public IDataResponse<EducationLevel> GetEducationLevels(Guid constituentId)
        {
            Repository<Constituent> repo = new Repository<Constituent>();
            var data = repo.Entities.Include(p => p.EducationLevel).FirstOrDefault(e => e.Id == constituentId)?.EducationLevel;

            var response = new DataResponse<EducationLevel> { Data = data };
            return response;
        }
        public IDataResponse AddConstituent(Constituent constituent)
        {
            var response = SafeExecute(() => { _repository.Insert(constituent); });
            return response;
        }

        public IDataResponse<int> GetNextConstituentNumber()
        {
            return new DataResponse<int>() { Data = _domain.GetNextConstituentNumber() };
        }

        private Type ConvertToType<T>(string property, T entity)
        {
            Type classType = entity.GetType();

            PropertyInfo[] properties = classType.GetProperties();

            var propertyType = properties.Where(p => p.Name == property).Select(p => p.PropertyType).Single();

            return propertyType;
    }
}
}

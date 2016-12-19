﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using DDI.Business.Domain;
using DDI.Business.Helpers;
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
                .SetOrderBy(search.OrderBy);
            // Created Range
            ApplyZipFilter(query, search);
            ApplyQuickFilter(query, search);

            var totalCount = query.GetQueryable().ToList().Count;

            query = query.SetLimit(search.Limit)
                         .SetOffset(search.Offset);

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            var response = GetIDataResponse(() => query.GetQueryable().ToList());
            response.TotalResults = totalCount;
            response.PageSize = search.Limit;
            response.PageNumber = search.Offset;
            response.Links = new List<HATEOASLink>()
            {
                new HATEOASLink()
                {
                    Href = search.ToQueryString(),
                    Relationship = "self",
                    Method = "GET"
                }
            };
            if (response.TotalResults > 1)
            {
                response.Links.AddRange(AddPagingLinks(search, response));
            }

            return response;
        }

        private List<HATEOASLink> AddPagingLinks(ConstituentSearch originalSearch, IDataResponse response)
        {
            var pagingLinks = new List<HATEOASLink>();

            pagingLinks.Add(CreateFirstPageLink(originalSearch, response));
            pagingLinks.Add(CreatePreviousPageLink(originalSearch, response));
            pagingLinks.Add(CreateNextPageLink(originalSearch, response));
            pagingLinks.Add(CreateLastPageLink(originalSearch, response));

            return pagingLinks;
        }

        private HATEOASLink CreateFirstPageLink(ConstituentSearch originalSearch, IDataResponse response)
        {
            HATEOASLink firstPageLink = null;
            firstPageLink = new HATEOASLink()
            {
                Href = $"{originalSearch.ToQueryString()}&offset=0&limit={originalSearch.Limit}&orderby={originalSearch.OrderBy}",
                Relationship = "first-page",
                Method = "GET"
            };
            
            return firstPageLink;
        }

        private HATEOASLink CreatePreviousPageLink(ConstituentSearch originalSearch, IDataResponse response)
        {
            HATEOASLink previousPageLink = null;
            if (originalSearch.Offset > 0)
            {
                previousPageLink = new HATEOASLink()
                {
                    Href = $"{originalSearch.ToQueryString()}&offset={--originalSearch.Offset ?? 0}&limit={originalSearch.Limit}&orderby={originalSearch.OrderBy}",
                    Relationship = "previous-page",
                    Method = "GET"
                };
            }

            return previousPageLink;
        }

        private HATEOASLink CreateNextPageLink(ConstituentSearch originalSearch, IDataResponse response)
        {
            HATEOASLink nextPageLink = null;
            if (originalSearch.Offset < response.TotalResults/originalSearch.Limit)
            {
                nextPageLink = new HATEOASLink()
                {
                    Href = $"{originalSearch.ToQueryString()}&offset={++originalSearch.Offset ?? 0}&limit={originalSearch.Limit}&orderby={originalSearch.OrderBy}",
                    Relationship = "next-page",
                    Method = "GET"
                };
            }

            return nextPageLink;
        }

        private HATEOASLink CreateLastPageLink(ConstituentSearch originalSearch, IDataResponse response)
        {
            HATEOASLink lastPageLink = null;
            lastPageLink = new HATEOASLink()
            {
                Href = $"{originalSearch.ToQueryString()}&offset={response.TotalResults / originalSearch.Limit}&limit={originalSearch.Limit}&orderby={originalSearch.OrderBy}",
                Relationship = "last-page",
                Method = "GET"
            };


            return lastPageLink;
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
                changedProperties.Add(pair.Key, pair.Value.ToObject(ConvertToType(pair.Key, typeof(Constituent))));
            }

            _repository.UpdateChangedProperties(id, changedProperties);

            var constituent = _repository.GetById(id);

            return GetIDataResponse(() => constituent);
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

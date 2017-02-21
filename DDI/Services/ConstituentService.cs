using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DDI.Business.CRM;
using DDI.Data;
using DDI.Search;
using DDI.Search.Models;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Logger;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;

namespace DDI.Services
{
    public class ConstituentService : ServiceBase<Constituent>, IConstituentService
    {
        private readonly IRepository<Constituent> _repository;
        private readonly ConstituentLogic _constituentlogic;
        private readonly Logger _logger;
        public ConstituentService()
            : this(new UnitOfWorkEF())
        {
        }

        public ConstituentService(IUnitOfWork uow)
            : this(uow, new ConstituentLogic(uow), uow.GetRepository<Constituent>())
        {
        }

        private ConstituentService(IUnitOfWork uow, ConstituentLogic constituentLogic, IRepository<Constituent> repository)
            : base(uow)
        {
            _constituentlogic = constituentLogic;
            _repository = repository;
        }

        private IDocumentSearchResult<ConstituentDocument> PerformElasticSearch(IPageable search)
        {
            var repo = new ElasticRepository<ConstituentDocument>();
            var query = repo.CreateQuery();

            var criteria = (ConstituentSearch)search;

            if (criteria.ConstituentNumber.HasValue && criteria.ConstituentNumber.Value > 0)
            {
                query.Must.Equal(criteria.ConstituentNumber.Value, p => p.ConstituentNumber);
            }
            else if (!string.IsNullOrWhiteSpace(criteria.QuickSearch))
            {
                query.Should.Boost(10).Equal(criteria.QuickSearch, p => p.ConstituentNumber);
                query.Should.Boost(7).Equal(criteria.QuickSearch, p => p.AlternateIds);
                query.Should.Boost(5).Match(criteria.QuickSearch, p => p.Name);
                query.Should.Boost(2).Match(criteria.QuickSearch, p => p.Name2, p => p.Nickname, p => p.PrimaryAddress);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(criteria.Name))
                {
                    query.Must.Match(criteria.Name, p => p.Name, p => p.Name2, p => p.Nickname, p => p.DoingBusinessAs);
                }

                if (!criteria.ConstituentTypeId.IsNullOrEmpty())
                {
                    query.Must.Equal(criteria.ConstituentTypeId.Value, p => p.ConstituentTypeId);
                }

                if (!string.IsNullOrWhiteSpace(criteria.AlternateId))
                {
                    query.Must.Equal(criteria.AlternateId, p => p.AlternateIds);
                }

                if (!string.IsNullOrWhiteSpace(criteria.IncludeTags))
                {
                    query.Must.BeInList(criteria.IncludeTags, p => p.Tags);
                }

                if (!string.IsNullOrWhiteSpace(criteria.ExcludeTags))
                {
                    query.MustNot.BeInList(criteria.ExcludeTags, p => p.Tags);
                }
                
                if (!string.IsNullOrWhiteSpace(criteria.Address) ||
                    !string.IsNullOrWhiteSpace(criteria.City) || 
                    !string.IsNullOrWhiteSpace(criteria.StateId) ||
                    !string.IsNullOrWhiteSpace(criteria.PostalCodeFrom) || 
                    !string.IsNullOrWhiteSpace(criteria.PostalCodeTo) ||
                    !string.IsNullOrWhiteSpace(criteria.CountryId) ||
                    !string.IsNullOrWhiteSpace(criteria.RegionId1) ||
                    !string.IsNullOrWhiteSpace(criteria.RegionId2) ||
                    !string.IsNullOrWhiteSpace(criteria.RegionId3) ||
                    !string.IsNullOrWhiteSpace(criteria.RegionId4)
                    )
                {
                    var addressQuery = new ElasticQuery<ConstituentDocument>();
                    if (!string.IsNullOrWhiteSpace(criteria.PostalCodeFrom) || !string.IsNullOrWhiteSpace(criteria.PostalCodeTo))
                    {
                        if (string.IsNullOrWhiteSpace(criteria.PostalCodeTo))
                        {
                            addressQuery.Must.Prefix(criteria.PostalCodeFrom, p => p.Addresses[0].PostalCode);
                        }
                        else if (string.IsNullOrWhiteSpace(criteria.PostalCodeFrom))
                        {
                            addressQuery.Must.Prefix(criteria.PostalCodeTo, p => p.Addresses[0].PostalCode);
                        }
                        else
                        {
                            addressQuery.Must.Range(criteria.PostalCodeFrom, criteria.PostalCodeTo + "~", p => p.Addresses[0].PostalCode);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(criteria.Address))
                    {
                        addressQuery.Must.Match(criteria.Address, p => p.Addresses[0].StreetAddress);
                    }
                    
                    if (!string.IsNullOrWhiteSpace(criteria.City))
                    {
                        addressQuery.Must.Match(criteria.City, p => p.Addresses[0].City);
                    }

                    if (!string.IsNullOrWhiteSpace(criteria.StateId))
                    {
                        addressQuery.Must.Equal(criteria.StateId, p => p.Addresses[0].StateId);
                    }

                    if (!string.IsNullOrWhiteSpace(criteria.CountryId))
                    {
                        addressQuery.Must.Equal(criteria.CountryId, p => p.Addresses[0].CountryId);
                    }

                    if (!string.IsNullOrWhiteSpace(criteria.RegionId1))
                    {
                        addressQuery.Must.Equal(criteria.RegionId1, p => p.Addresses[0].Region1Id);
                    }

                    if (!string.IsNullOrWhiteSpace(criteria.RegionId2))
                    {
                        addressQuery.Must.Equal(criteria.RegionId2, p => p.Addresses[0].Region1Id);
                    }

                    if (!string.IsNullOrWhiteSpace(criteria.RegionId3))
                    {
                        addressQuery.Must.Equal(criteria.RegionId3, p => p.Addresses[0].Region1Id);
                    }

                    if (!string.IsNullOrWhiteSpace(criteria.RegionId4))
                    {
                        addressQuery.Must.Equal(criteria.RegionId4, p => p.Addresses[0].Region1Id);
                    }

                    query.Must.Nested(p => p.Addresses, addressQuery);
                }
            }

            switch (search.OrderBy)
            {
                case OrderByProperties.DisplayName:
                    query.OrderBy(p => p.SortableName);
                    break;
            }

            return repo.DocumentSearch(query, search.Limit ?? 0, search.Offset ?? 0);
        }

        public override IDataResponse<List<ICanTransmogrify>> GetAll(string fields, IPageable search)
        {
            var criteria = (ConstituentSearch)search;

            if (criteria.ConstituentNumber > 0)
            {
                var constituents = UnitOfWork.GetEntities(IncludesForList).Where(p => p.ConstituentNumber == criteria.ConstituentNumber.Value);
                return new DataResponse<List<ICanTransmogrify>>(constituents.ToList<ICanTransmogrify>()) { TotalResults = constituents.Count() };
            }

            IDocumentSearchResult<ConstituentDocument> results = PerformElasticSearch(search);

            if (VerifyFieldList<ConstituentDocument>(fields))
            {
                return new DataResponse<List<ICanTransmogrify>>(results.Documents.ToList<ICanTransmogrify>()) { TotalResults = results.TotalCount };
            }

            List<Guid> ids = results.Documents.Select(p => p.Id).ToList();
            List<ICanTransmogrify> constituentsFound = ids.Join(UnitOfWork.GetEntities(IncludesForList).Where(p => ids.Contains(p.Id)), outer => outer, inner => inner.Id,
                (id, constituent) => constituent).ToList<ICanTransmogrify>();
               
            return new DataResponse<List<ICanTransmogrify>>(constituentsFound) { TotalResults = results.TotalCount };
        }

        public IDataResponse<List<Constituent>> GetAllOld(IPageable search)
        { 
            var constituentSearch = (ConstituentSearch) search;
            IQueryable<Constituent> constituents = _repository.GetEntities(IncludesForList);
            var query = new CriteriaQuery<Constituent, ConstituentSearch>(constituents, constituentSearch)
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.ConstituentNumber, c => c.ConstituentNumber)
                .IfModelPropertyIsNotBlankAndDatabaseContainsIt(m => m.Name, c => c.FormattedName)
                .IfModelPropertyIsNotBlankThenAndTheExpression(m => m.City, c => c.ConstituentAddresses.Any(a => a.Address.City.StartsWith(constituentSearch.City)))
                .IfModelPropertyIsNotBlankThenAndTheExpression(m => m.AlternateId, c => c.AlternateIds.Any(a => a.Name.Contains(constituentSearch.AlternateId)))
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.ConstituentTypeId, c => c.ConstituentTypeId);
            
            // Created Range
            ApplyZipFilter(query, constituentSearch);
            ApplyQuickFilter(query, constituentSearch);

            if (!string.IsNullOrWhiteSpace(search.OrderBy) && search.OrderBy != OrderByProperties.DisplayName)
            {
                query = query.SetOrderBy(search.OrderBy);
            }

            var totalCount = query.GetQueryable().ToList().Count;

            query = query.SetLimit(search.Limit)
                         .SetOffset(search.Offset);

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            var response = GetIDataResponse(() => query.GetQueryable().ToList());
            if (search.OrderBy == OrderByProperties.DisplayName)
            {
                response.Data = response.Data.OrderBy(a => a.DisplayName).ToList();
            }

            response.TotalResults = totalCount;

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
            if (!search.PostalCodeFrom.IsNullOrWhiteSpace() || !search.PostalCodeTo.IsNullOrWhiteSpace())
            {
                if (search.PostalCodeFrom.IsNullOrWhiteSpace())
                {
                    query.And(c => c.ConstituentAddresses.Any(a => a.Address.PostalCode.StartsWith(search.PostalCodeTo)));
                }
                else if (search.PostalCodeTo.IsNullOrWhiteSpace())
                {
                    query.And(c => c.ConstituentAddresses.Any(a => a.Address.PostalCode.StartsWith(search.PostalCodeFrom)));
                }
                else
                {
                    query.And(c => (c.ConstituentAddresses.Any(a => a.Address.PostalCode.CompareTo(search.PostalCodeFrom) >= 0 && a.Address.PostalCode.CompareTo(search.PostalCodeTo + "~") <= 0)));
                }
            }
        }

        public IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum)
        {
            var constituent = _repository.Entities.FirstOrDefault(c => c.ConstituentNumber == constituentNum);
            constituent = _constituentlogic.ConvertAgeRange(constituent);
            return GetById(constituent?.Id ?? Guid.Empty);
        }

        public IDataResponse<Constituent> NewConstituent(Guid constituentTypeId)
        {            
            var constituentType = UnitOfWork.GetRepository<ConstituentType>().GetById(constituentTypeId);
            if (constituentType == null)
            {
                throw new ArgumentException("Constituent type ID is not valid.");               
            }

            var constituent = new Constituent();
            constituent.ConstituentNumber = _constituentlogic.GetNextConstituentNumber();
            constituent.ConstituentType = constituentType;

            // ToDo:  Other new constituent tasks, such as initial tags, status code.

            return new DataResponse<Constituent>() { Data = constituent };

        }

        public override IDataResponse<Constituent> Add(Constituent entity)
        {
            entity = _constituentlogic.ConvertAgeRange(entity);
            return base.Add(entity);
        }

        public override IDataResponse Update(Constituent entity)
        {
            entity = _constituentlogic.ConvertAgeRange(entity);
            return base.Update(entity);
        }

        public override IDataResponse<Constituent> Update(Guid id, JObject changes)
        {
            foreach (var change in changes)
            {
                if (change.Key == nameof(Constituent.BirthYearFrom) || change.Key == nameof(Constituent.BirthYearTo))
                {
                    if (change.Value != null)
                    {
                        try
                        {
                            _constituentlogic.ConvertAgeRange((int)change.Value);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex);
                        }
                    }
                }
            }
            return base.Update(id, changes);
        }
    }
}

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

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public override IDataResponse<List<ICanTransmogrify>> GetAll(string fields, IPageable search = null)
        {
            if (!VerifyFieldList<ConstituentDocument>(fields))
            {
                return null;
            }

            IDocumentSearchResult<ConstituentDocument> results = PerformElasticSearch(search);

            return new DataResponse<List<ICanTransmogrify>>(results.Documents.ToList<ICanTransmogrify>()) { TotalResults = results.TotalCount };
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

            }

            switch (search.OrderBy)
            {
                case OrderByProperties.DisplayName:
                    query.OrderBy(p => p.SortableName);
                    break;
            }

            return repo.DocumentSearch(query, search.Limit ?? 0, search.Offset ?? 0);
        }

        public override IDataResponse<List<Constituent>> GetAll(IPageable search)
        {
            IDocumentSearchResult<ConstituentDocument> results = PerformElasticSearch(search);
            List<Guid> ids = results.Documents.Select(p => p.Id).ToList();
            List<Constituent> constituentsFound = ids.Join(UnitOfWork.GetEntities(IncludesForList).Where(p => ids.Contains(p.Id)), outer => outer, inner => inner.Id,
                (id, constituent) => constituent).ToList();
               
            return new DataResponse<List<Constituent>>(constituentsFound) { TotalResults = results.TotalCount };
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

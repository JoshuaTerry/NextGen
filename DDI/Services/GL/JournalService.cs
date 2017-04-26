using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Search;
using DDI.Search.Models;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;

namespace DDI.Services.GL
{
    public class JournalService : ServiceBase<Journal>, IJournalService
    {

        public override IDataResponse<List<ICanTransmogrify>> GetAll(string fields, IPageable search)
        {
            var criteria = (JournalSearch)search;

            // If journal number specified, bypass all other search criteria and retrieve single journal.
            if (criteria.JournalNumber > 0 && criteria.JournalType.HasValue)
            {
                if (criteria.JournalType.Value == Shared.Enums.GL.JournalType.Normal && criteria.FiscalYearId.HasValue)
                {
                    var journals = UnitOfWork.GetEntities(IncludesForList).Where(p => p.JournalType == criteria.JournalType.Value && p.FiscalYearId == criteria.FiscalYearId.Value && p.JournalNumber == criteria.JournalNumber);
                    return new DataResponse<List<ICanTransmogrify>>(journals.ToList<ICanTransmogrify>()) { TotalResults = journals.Count() };
                }
                else if (criteria.JournalType.Value != Shared.Enums.GL.JournalType.Normal && criteria.BusinessUnitId.HasValue)
                {
                    var journals = UnitOfWork.GetEntities(IncludesForList).Where(p => p.JournalType == criteria.JournalType.Value && p.BusinessUnitId == criteria.BusinessUnitId.Value && p.JournalNumber == criteria.JournalNumber);
                    return new DataResponse<List<ICanTransmogrify>>(journals.ToList<ICanTransmogrify>()) { TotalResults = journals.Count() };
                }
            }

            // Using Elasticsearch to retrieve documents.
            IDocumentSearchResult<JournalDocument> results = PerformElasticSearch(search as JournalSearch);

            // If the field list can be satisfied via the search documents, return them.
            if (VerifyFieldList<JournalDocument>(fields))
            {
                return new DataResponse<List<ICanTransmogrify>>(results.Documents.ToList<ICanTransmogrify>()) { TotalResults = results.TotalCount };
            }

            // Otherwise, convert the search documents to journals via a join.
            List<Guid> ids = results.Documents.Select(p => p.Id).ToList(); // This is the list of Ids to retrieve.  See the Where method below that filters journals via this list.
            List<ICanTransmogrify> journalsFound = ids.Join(UnitOfWork.GetEntities(IncludesForList).Where(p => ids.Contains(p.Id)),
                                                                outer => outer,
                                                                inner => inner.Id,
                                                                (id, journal) => journal)
                                                          .ToList<ICanTransmogrify>();

            return new DataResponse<List<ICanTransmogrify>>(journalsFound) { TotalResults = results.TotalCount };
        }       
        
        private IDocumentSearchResult<JournalDocument> PerformElasticSearch(JournalSearch search)
        {
            bool ignoreFiscalYear = false;
            bool ignoreBusinessUnit = false;

            if (search == null)
            {
                return new DocumentSearchResult<JournalDocument>();
            }

            var repo = new ElasticRepository<JournalDocument>();
            var query = repo.CreateQuery();

            // Build the ElasticSearch query based on journal search criteria.
            if (search.JournalNumber > 0)
            {
                query.Must.Equal(search.JournalNumber, p => p.JournalNumber);
            }

            if (search.JournalType.HasValue)
            {
                query.Must.Equal((int)search.JournalType.Value, p => p.JournalType);
                if (search.JournalType.Value == Shared.Enums.GL.JournalType.Normal && search.FiscalYearId.HasValue)
                {
                    ignoreBusinessUnit = true;
                }
                else
                {
                    ignoreFiscalYear = true;
                } 
            }

            if (!ignoreBusinessUnit && search.BusinessUnitId.HasValue)
            {
                query.Must.Equal(search.BusinessUnitId.Value.ToString("d"), p => p.BusinessUnitId);
            }

            if (!ignoreFiscalYear && search.FiscalYearId.HasValue)
            {
                query.Must.Equal(search.FiscalYearId.Value.ToString("d"), p => p.FiscalYearId);
            }

            if (search.AmountFrom > 0 || search.AmountTo > 0)
            {
                if (search.AmountTo <= 0)
                {
                    search.AmountTo = 999999999999m;
                }
                query.Must.Range((double)search.AmountFrom, (double)search.AmountTo, p => p.Amount);
            }

            if (!string.IsNullOrWhiteSpace(search.Comment))
            {
                query.Must.Match(search.Comment, p => p.Comment);
            }

            if (!string.IsNullOrWhiteSpace(search.LineItemComment))
            {
                query.Must.Match(search.LineItemComment, p => p.LineItemComments);
            }

            if (!string.IsNullOrWhiteSpace(search.JournalStatus))
            {
                query.Must.BeInList(search.JournalStatus, p => p.Status);
            }

            if (search.TransactionDateFrom.HasValue || search.TransactionDateTo.HasValue)
            {
                query.Must.Range(search.TransactionDateFrom ?? DateTime.MinValue, search.TransactionDateTo ?? DateTime.MaxValue, p => p.TransactionDate);
            }

            if (!string.IsNullOrWhiteSpace(search.CreatedBy))
            {
                query.Must.Match(search.CreatedBy, p => p.CreatedBy);
            }

            if (search.CreatedOnFrom.HasValue || search.CreatedOnTo.HasValue)
            {
                query.Must.Range(search.CreatedOnFrom ?? DateTime.MinValue, search.CreatedOnTo ?? DateTime.MaxValue, p => p.CreatedOn);
            }

            switch (search.OrderBy)
            {
                case OrderByProperties.EntityNumber:
                    query.OrderBy(p => p.JournalNumber);
                    break;
                case OrderByProperties.TransactionDate:
                    query.OrderByDescending(p => p.TransactionDate);                    
                    break;
            }

            return repo.DocumentSearch(query, search.Limit ?? 0, search.Offset ?? 0);
            
        }
    }
}

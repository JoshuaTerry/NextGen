using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DDI.Business.CRM;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Search;
using DDI.Search.Models;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using WebGrease.Css.Extensions;

namespace DDI.Services
{
    public class ConstituentService : ServiceBase<Constituent>, IConstituentService
    {
        #region Private Fields

        private readonly IRepository<Constituent> _repository;
        private readonly ConstituentLogic _constituentlogic; 

        #endregion

        #region Constructors

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

        #endregion

        #region Public Methods

        protected override Action<Constituent> FormatEntityForGet => p => SetDateTimeKind(p, q => q.ConstituentStatusDate);

        public override IDataResponse<List<ICanTransmogrify>> GetAll(string fields, IPageable search)
        {
            var criteria = (ConstituentSearch)search;

            // If constituent number specified, bypass all other search criteria and retrieve single constituent.
            if (criteria.ConstituentNumber > 0)
            {
                var constituents = UnitOfWork.GetEntities(IncludesForList).Where(p => p.ConstituentNumber == criteria.ConstituentNumber.Value);
                return new DataResponse<List<ICanTransmogrify>>(constituents.ToList<ICanTransmogrify>()) { TotalResults = constituents.Count() };
            }

            // Using Elasticsearch to retrieve documents.
            IDocumentSearchResult<ConstituentDocument> results = PerformElasticSearch(search as ConstituentSearch);

            // If the field list can be satisfied via the search documents, return them.
            if (VerifyFieldList<ConstituentDocument>(fields))
            {
                return new DataResponse<List<ICanTransmogrify>>(results.Documents.ToList<ICanTransmogrify>()) { TotalResults = results.TotalCount };
            }

            // Otherwise, convert the search documents to constituents via a join.
            List<Guid> ids = results.Documents.Select(p => p.Id).ToList(); // This is the list of Ids to retrieve.  See the Where method below that filters constituents via this list.
            List<ICanTransmogrify> constituentsFound = ids.Join(UnitOfWork.GetEntities(IncludesForList).Where(p => ids.Contains(p.Id)), 
                                                                outer => outer, 
                                                                inner => inner.Id,
                                                                (id, constituent) => constituent)
                                                          .ToList<ICanTransmogrify>();
               
            return new DataResponse<List<ICanTransmogrify>>(constituentsFound) { TotalResults = results.TotalCount };
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
                            Logger.LogError(ex.ToString());
                        }
                    }
                }
            }
            return base.Update(id, changes);
        }

        public IDataResponse AddTagsToConstituent(Constituent constituent, JObject tagIds)
        {
            var constituentToUpdate = UnitOfWork.GetById<Constituent>(constituent.Id, c => c.Tags);
            IDataResponse response = null;
            List<Tag> passedTags = new List<Tag>();
            List<Tag> constituentTags = new List<Tag>();

            foreach (var pair in tagIds)
            {
                if (pair.Value.Type == JTokenType.Array && pair.Value.HasValues)
                {
                    passedTags.AddRange(from jToken in (JArray)pair.Value select Guid.Parse(jToken.ToString()) into id select UnitOfWork.GetById<Tag>(id));
                }
            }

            constituentTags = constituentToUpdate.Tags.ToList();

            var removes = constituentTags.Except(passedTags);
            var adds = passedTags.Except(constituentTags);

            if (constituentToUpdate != null)
            {
                removes.ForEach(r => constituentToUpdate.Tags.Remove(r));
                adds.ForEach(a => constituentToUpdate.Tags.Add(a));
            }

            UnitOfWork.SaveChanges();
            
            response = new DataResponse<Constituent>()
            {
                Data = UnitOfWork.GetById<Constituent>(constituent.Id),
                IsSuccessful = true
                
            };

            return response;
        }

        public IDataResponse RemoveTagFromConstituent(Constituent constituent, Guid tagId)
        {
            IDataResponse response = null;
            var tagToRemove = constituent.Tags.Where(t => t.Id == tagId).FirstOrDefault();

            if (tagToRemove != null)
            {
                constituent.Tags.Remove(tagToRemove);
            }

            UnitOfWork.SaveChanges();

            response = new DataResponse<Constituent>()
            {
                Data = UnitOfWork.GetById<Constituent>(constituent.Id),
                IsSuccessful = true

            };

            return response;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Use ElasticSearch to search for constituents.
        /// </summary>
        /// <param name="search">ConstituentSearch instance.</param>
        private IDocumentSearchResult<ConstituentDocument> PerformElasticSearch(ConstituentSearch search)
        {
            bool isScoring = false;

            if (search == null)
            {
                return new DocumentSearchResult<ConstituentDocument>();
            }

            var repo = new ElasticRepository<ConstituentDocument>();
            var query = repo.CreateQuery();

            if (search.ConstituentNumber.HasValue && search.ConstituentNumber.Value > 0)
            {
                query.Must.Equal(search.ConstituentNumber.Value, p => p.ConstituentNumber);
            }
            else if (!string.IsNullOrWhiteSpace(search.QuickSearch))
            {
                query.Should.Boost(5).Equal(search.QuickSearch, p => p.AlternateIds);
                query.Should.Boost(1.5).Match(search.QuickSearch, p => p.Name);
                query.Should.Boost(1).Match(search.QuickSearch, p => p.Name2, p => p.Nickname, p => p.PrimaryAddress);
                isScoring = true;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(search.Name))
                {
                    query.Must.MatchAll(search.Name, p => p.Name, p => p.Name2, p => p.Nickname, p => p.DoingBusinessAs);
                }

                if (!search.ConstituentTypeId.IsNullOrEmpty())
                {
                    query.Must.Equal(search.ConstituentTypeId.Value, p => p.ConstituentTypeId);
                }

                if (!string.IsNullOrWhiteSpace(search.AlternateId))
                {
                    query.Must.Equal(search.AlternateId, p => p.AlternateIds);
                }

                string includeTags = CodeEntityHelper.ConvertCodeListToGuidList<Tag>(UnitOfWork, search.IncludeTags);
                if (includeTags != null)
                {
                    query.Must.BeInList(includeTags, p => p.Tags);
                }

                string excludeTags = CodeEntityHelper.ConvertCodeListToGuidList<Tag>(UnitOfWork, search.ExcludeTags);
                if (!string.IsNullOrWhiteSpace(excludeTags))
                {
                    query.MustNot.BeInList(excludeTags, p => p.Tags);
                }

                if (search.CreatedDateFrom.HasValue || search.CreatedDateTo.HasValue)
                {
                    DateTime from = search.CreatedDateFrom ?? new DateTime(1800, 1, 1);
                    DateTime to = search.CreatedDateTo ?? new DateTime(2999, 12, 31);
                    if (from > to)
                    {
                        DateTime temp = from;
                        from = to;
                        to = temp;
                    }

                    query.Must.Range(from, to, p => p.CreationDate);
                }

                if (search.AgeFrom > 0 || search.AgeTo > 0)
                {
                    // Age range: 12-16
                    // Year: 2017
                    // Year range: 2001 - 2005
                    int year = DateTime.Now.Year;
                    if (search.AgeTo > 0)
                    {
                        query.Must.Range(year - search.AgeTo.Value, year, p => p.BirthYearFrom);
                    }

                    if (search.AgeFrom > 0)
                    {
                        query.Must.Range(1, year - search.AgeFrom.Value, p => p.BirthYearTo);
                    }
                }

                if (!string.IsNullOrWhiteSpace(search.Address) ||
                    !string.IsNullOrWhiteSpace(search.City) ||
                    !string.IsNullOrWhiteSpace(search.StateId) ||
                    !string.IsNullOrWhiteSpace(search.PostalCodeFrom) ||
                    !string.IsNullOrWhiteSpace(search.PostalCodeTo) ||
                    !string.IsNullOrWhiteSpace(search.CountryId) ||
                    !string.IsNullOrWhiteSpace(search.RegionId1) ||
                    !string.IsNullOrWhiteSpace(search.RegionId2) ||
                    !string.IsNullOrWhiteSpace(search.RegionId3) ||
                    !string.IsNullOrWhiteSpace(search.RegionId4)
                    )
                {
                    var addressQuery = new ElasticQuery<ConstituentDocument>();
                    if (!string.IsNullOrWhiteSpace(search.PostalCodeFrom) || !string.IsNullOrWhiteSpace(search.PostalCodeTo))
                    {
                        if (string.IsNullOrWhiteSpace(search.PostalCodeTo))
                        {
                            addressQuery.Must.Prefix(search.PostalCodeFrom, p => p.Addresses[0].PostalCode);
                        }
                        else if (string.IsNullOrWhiteSpace(search.PostalCodeFrom))
                        {
                            addressQuery.Must.Prefix(search.PostalCodeTo, p => p.Addresses[0].PostalCode);
                        }
                        else
                        {
                            addressQuery.Must.Range(search.PostalCodeFrom, search.PostalCodeTo + "~", p => p.Addresses[0].PostalCode);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(search.Address))
                    {
                        addressQuery.Must.Match(search.Address, p => p.Addresses[0].StreetAddress);
                    }

                    if (!string.IsNullOrWhiteSpace(search.City))
                    {
                        addressQuery.Must.Match(search.City, p => p.Addresses[0].City);
                    }

                    if (!string.IsNullOrWhiteSpace(search.StateId))
                    {
                        addressQuery.Must.Equal(search.StateId, p => p.Addresses[0].StateId);
                    }

                    if (!string.IsNullOrWhiteSpace(search.CountryId))
                    {
                        addressQuery.Must.Equal(search.CountryId, p => p.Addresses[0].CountryId);
                    }

                    if (!string.IsNullOrWhiteSpace(search.RegionId1))
                    {
                        Guid? id = CodeEntityHelper.ConvertToGuid<Region>(UnitOfWork, search.RegionId1, p => p.Level == 1);
                        if (id != null)
                        {
                            addressQuery.Must.Equal(id.Value, p => p.Addresses[0].Region1Id);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(search.RegionId2))
                    {
                        Guid? id = CodeEntityHelper.ConvertToGuid<Region>(UnitOfWork, search.RegionId2, p => p.Level == 2);
                        if (id != null)
                        {
                            addressQuery.Must.Equal(id.Value, p => p.Addresses[0].Region2Id);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(search.RegionId3))
                    {
                        Guid? id = CodeEntityHelper.ConvertToGuid<Region>(UnitOfWork, search.RegionId3, p => p.Level == 3);
                        if (id != null)
                        {
                            addressQuery.Must.Equal(id.Value, p => p.Addresses[0].Region3Id);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(search.RegionId4))
                    {
                        Guid? id = CodeEntityHelper.ConvertToGuid<Region>(UnitOfWork, search.RegionId4, p => p.Level == 4);
                        if (id != null)
                        {
                            addressQuery.Must.Equal(id.Value, p => p.Addresses[0].Region4Id);
                        }
                    }

                    query.Must.Nested(p => p.Addresses, addressQuery);
                }
            }

            if (isScoring)
            {
                query.OrderByScore();
            }

            switch (search.OrderBy)
            {
                case OrderByProperties.DisplayName:
                    query.OrderBy(p => p.SortableName);
                    break;
            }
            

            return repo.DocumentSearch(query, search.Limit ?? 0, search.Offset ?? 0);
        }



        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using DDI.Data;
using DDI.Shared;
using Newtonsoft.Json.Linq; 
using DDI.Business.CRM;
using Microsoft.Ajax.Utilities;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;
using DDI.Shared.Statics;

namespace DDI.Services
{
    public class ConstituentService : ServiceBase<Constituent>, IConstituentService
    {
        private readonly IRepository<Constituent> _repository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ConstituentLogic _constituentlogic;

        public ConstituentService()
            : this(new UnitOfWorkEF())
        {
        }


        public ConstituentService(IUnitOfWork uow)
            : this(uow, new ConstituentLogic(uow), uow.GetRepository<Constituent>())
        {
        }

        private ConstituentService(IUnitOfWork uow, ConstituentLogic constituentLogic, IRepository<Constituent> repository )
        {
            _unitOfWork = uow;
            _constituentlogic = constituentLogic;
            _repository = repository;
        }
        
        public override IDataResponse<List<Constituent>> GetAll(IPageable search)
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

        public override IDataResponse<Constituent> GetById(Guid id)
        {
            Constituent constituent = _repository.GetById(id, IncludesForSingle);

            var response = GetIDataResponse(() => constituent);
            return response;
        }

        public IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum)
        {
            var constituent = _repository.Entities.FirstOrDefault(c => c.ConstituentNumber == constituentNum);
            return GetById(constituent?.Id ?? Guid.Empty);
        }

        public IDataResponse<Constituent> UpdateConstituent(Guid id, JObject changes)
        {
            Dictionary<string, object> changedProperties = new Dictionary<string, object>();

            foreach (var pair in changes)
            {
                changedProperties.Add(pair.Key, pair.Value.ToObject(ConvertToType<Constituent>(pair.Key)));
            }

            _repository.UpdateChangedProperties(id, changedProperties, p =>
            {
                _constituentlogic.Validate(p);
            });

            _unitOfWork.SaveChanges();

            return GetById(id);
        }

        public IDataResponse<Constituent> AddConstituent(Constituent constituent)
        {
            try
            {
                _constituentlogic.Validate(constituent);
                _repository.Insert(constituent);
                _unitOfWork.SaveChanges();

                return GetById(constituent.Id);
            }
            catch (Exception ex)
            {
                return ProcessIDataResponseException(ex);
            };
        }

        public IDataResponse<Constituent> NewConstituent(Guid constituentTypeId)
        {            
            var constituentType = _unitOfWork.GetRepository<ConstituentType>().GetById(constituentTypeId);
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

        private Type ConvertToType<T>(string property)
        {
            Type classType = typeof(T);

            var propertyType = classType.GetProperty(property).PropertyType;

            return propertyType;
        }

        public IDataResponse<int> GetNextConstituentNumber()
        {
            throw new NotImplementedException();
        }

        object IConstituentService.NewConstituent(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

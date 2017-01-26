using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DDI.Data;
using DDI.Shared;
using Newtonsoft.Json.Linq; 
using DDI.Business.CRM;
using Microsoft.Ajax.Utilities;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;

namespace DDI.Services
{
    public class ConstituentService : ServiceBase<Constituent>, IConstituentService
    {
        private IRepository<Constituent> _repository;

        private IUnitOfWork _unitOfWork;
        private ConstituentLogic _constituentlogic;

        public ConstituentService()
        {
            Initialize(new UnitOfWorkEF());
        }


        public ConstituentService(IUnitOfWork uow)
        {
            Initialize(uow);
        }

        private void Initialize(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _constituentlogic = new ConstituentLogic(_unitOfWork);
            _repository = _unitOfWork.GetRepository<Constituent>();
        }
        
        public IDataResponse<List<Constituent>> GetConstituents(ConstituentSearch search)
        {
            IQueryable<Constituent> constituents = _repository.Entities.Include("ConstituentAddresses.Address");
            var query = new CriteriaQuery<Constituent, ConstituentSearch>(constituents, search)
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.ConstituentNumber, c => c.ConstituentNumber)
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
            Constituent constituent = _repository.GetById(id,
                c => c.ClergyStatus,
                c => c.ClergyType,
                c => c.ConstituentStatus,
                c => c.ConstituentType,
                c => c.EducationLevel,
                c => c.Gender,
                c => c.IncomeLevel,
                c => c.Language,
                c => c.MaritalStatus,
                c => c.Prefix,
                c => c.Profession
                );
            var response = GetIDataResponse(() => constituent);

            return response;
        }

        public IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum)
        {
            var constituent = _repository.Entities.FirstOrDefault(c => c.ConstituentNumber == constituentNum);
            var response = GetIDataResponse(() => constituent);
            return response;
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

            var constituent = _repository.GetById(id);

            return GetIDataResponse(() => constituent);
        }

        public IDataResponse<List<DoingBusinessAs>> GetConstituentDBAs(Guid constituentId)
        {
            Repository<DoingBusinessAs> dbaRepo = new Repository<DoingBusinessAs>();
            var data = dbaRepo.Entities.Where(d => d.ConstituentId == constituentId);

            IDataResponse<List<DoingBusinessAs>> response = new DataResponse<List<DoingBusinessAs>> { Data = data.ToList() };
            return response;
        }

        public IDataResponse<List<ConstituentAddress>> GetConstituentAddresses(Guid constituentId)
        {
            Repository<ConstituentAddress> dbaRepo = new Repository<ConstituentAddress>();
            var data = dbaRepo.Entities.Where(d => d.ConstituentId == constituentId);

            IDataResponse<List<ConstituentAddress>> response = new DataResponse<List<ConstituentAddress>> { Data = data.ToList() };
            return response;
        }

        public IDataResponse<EducationLevel> GetEducationLevel(Guid constituentId)
        {
            Repository<Constituent> repo = new Repository<Constituent>();
            var data = repo.Entities.Include(p => p.EducationLevel).FirstOrDefault(e => e.Id == constituentId)?.EducationLevel;

            IDataResponse<EducationLevel> response = new DataResponse<EducationLevel> { Data = data };
            return response;
        }

        public IDataResponse AddConstituent(Constituent constituent)
        {
            var response = SafeExecute(() => 
            {
                _constituentlogic.Validate(constituent);
                _repository.Insert(constituent);
                _unitOfWork.SaveChanges();
            });
            return response;
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

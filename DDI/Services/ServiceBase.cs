using DDI.Data;
using DDI.Shared;
using DDI.Shared.Logger;
using DDI.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Services.Search;

namespace DDI.Services
{
    public class ServiceBase<T> where T : class, IEntity
    {
        private static readonly Logger _logger = Logger.GetLogger(typeof(ServiceBase<T>));
        private readonly IUnitOfWork _unitOfWork; 

        public ServiceBase() : this(new UnitOfWorkEF())
        {            
        }
        public ServiceBase(IUnitOfWork uow)
        {
            _unitOfWork = uow;  
        }

        protected IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }
        public IDataResponse<List<T>> GetAll(IPageable search = null)
        {
            if (search == null)
            {
                search = new PageableSearch
                {
                    Limit = 25,
                    Offset = 0
                };
            }

            IQueryable<T> queryable = _unitOfWork.GetRepository<T>().Entities;
            var query = new CriteriaQuery<T, IPageable>(queryable, search);

            if (!string.IsNullOrWhiteSpace(search.OrderBy) && search.OrderBy != "DisplayName")
            {
                query = query.SetOrderBy(search.OrderBy);
            }

            var totalCount = query.GetQueryable().ToList().Count;

            query = query.SetLimit(search.Limit)
                         .SetOffset(search.Offset);

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            var response = GetIDataResponse(() => query.GetQueryable().ToList());
            if (search.OrderBy == "DisplayName")
            {
                response.Data = response.Data.OrderBy(a => a.DisplayName).ToList();
            }

            response.TotalResults = totalCount;

            return response;
        }

        public IDataResponse<T> GetById(Guid id)
        {
            var result = _unitOfWork.GetRepository<T>().GetById(id); 
            return GetIDataResponse(() => result);
        }

        public IDataResponse Update(T entity)
        {
            var response = new DataResponse<T>();
            try
            {
                response.Data = _unitOfWork.GetRepository<T>().Update(entity);
            }
            catch (Exception ex)
            {
                return ProcessIDataResponseException(ex);
            }

            return response;
        }

        public IDataResponse<T> Update(Guid id, JObject changes)
        {
            var response = new DataResponse<T>();
            Dictionary<string, object> changedProperties = new Dictionary<string, object>();
            try
            {
                foreach (var pair in changes)
                {
                    changedProperties.Add(pair.Key, pair.Value.ToObject(ConvertToType<T>(pair.Key)));
                }

                _unitOfWork.GetRepository<T>().UpdateChangedProperties(id, changedProperties);

                response.Data = _unitOfWork.GetRepository<T>().GetById(id);
            }
            catch (Exception ex)
            {
                return ProcessIDataResponseException(ex);
            }

            return response;
        }

        public IDataResponse<T> Add(T entity)
        {
            var response = new DataResponse<T>();
            try
            {
                response.Data = _unitOfWork.GetRepository<T>().Insert(entity);
            }
            catch (Exception ex)
            {
                return ProcessIDataResponseException(ex);
            }

            return response;
        }

        public IDataResponse Delete(T entity)
        {
            var response = new DataResponse();
            try
            {
                _unitOfWork.GetRepository<T>().Delete(entity);
            }
            catch (Exception ex)
            {
                return ProcessIDataResponseException(ex);
            }

            return response;
        }

        private Type ConvertToType<T1>(string property)
        {
            Type classType = typeof(T1);

            var propertyType = classType.GetProperty(property).PropertyType;

            return propertyType;
        }


        public IDataResponse<T1> GetIDataResponse<T1>(Func<T1> funcToExecute, string fieldList = null, bool shouldAddLinks = false)
        {
            return GetDataResponse(funcToExecute, fieldList, shouldAddLinks);
        }

        public DataResponse<T1> GetDataResponse<T1>(Func<T1> funcToExecute, string fieldList = null, bool shouldAddLinks = false)
        {
            try
            {
                var result = funcToExecute();
                var response = new DataResponse<T1>
                {
                    Data = result,
                    IsSuccessful = true
                };
                return response;
            }
            catch (Exception ex)
            {
                return ProcessDataResponseException<T1>(ex);
            }
        }

        public IDataResponse GetDataResponse(Action actionToExecute)
        {
            DataResponse response;

            try
            {
                actionToExecute();
                response = new DataResponse
                {
                    IsSuccessful = true
                };
            }
            catch (Exception ex)
            {
                return ProcessIDataResponseException(ex);
            }

            return response;
        }

        public IDataResponse<T1> GetErrorResponse<T1>(string errorMessage, string verboseErrorMessage = null)
        {
            _logger.Error($"Message: {errorMessage} | Verbose Message: {verboseErrorMessage}");

            return (verboseErrorMessage == null)
                ? GetErrorResponse<T1>(new List<string> { errorMessage })
                : GetErrorResponse<T1>(new List<string> { errorMessage }, new List<string> { verboseErrorMessage });
        }

        public IDataResponse<T1> GetErrorResponse<T1>(IEnumerable<string> errorMessages, IEnumerable<string> verboseErrorMessages = null)
        {
            return new DataResponse<T1>
            {
                IsSuccessful = false,
                Data = default(T1),
                ErrorMessages = errorMessages?.ToList(),
                VerboseErrorMessages = verboseErrorMessages?.ToList()
            };
        }

        public IDataResponse<T> ProcessIDataResponseException(Exception ex)
        {
            var response = new DataResponse<T>();
            response.IsSuccessful = false;
            response.ErrorMessages.Add(ex.Message);
            response.VerboseErrorMessages.Add(ex.ToString());
            _logger.Error(ex);

            return response;
        }

        public DataResponse<T1> ProcessDataResponseException<T1>(Exception ex)
        {
            var response = new DataResponse<T1>();
            response.IsSuccessful = false;
            response.ErrorMessages.Add(ex.Message);
            response.VerboseErrorMessages.Add(ex.ToString());
            _logger.Error(ex);

            return response;

        }
    }
}

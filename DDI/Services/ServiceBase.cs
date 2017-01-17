using DDI.Data;
using DDI.Shared;
using DDI.Shared.Logger;
using DDI.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public IDataResponse<List<T>> GetAll()
        {
            var result = _unitOfWork.GetRepository<T>().Entities.ToList().OrderBy(a => a.DisplayName).ToList();
            return GetIDataResponse(() => result);
        }

        public IDataResponse<T> GetById(Guid id)
        {
            var result = _unitOfWork.GetRepository<T>().GetById(id); 
            return GetIDataResponse(() => result);
        }

        public IDataResponse Update(T entity)
        {
            var response = new DataResponse();
            try
            {
                _unitOfWork.GetRepository<T>().Update(entity);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public IDataResponse<T> Update(Guid id, JObject changes)
        {
            Dictionary<string, object> changedProperties = new Dictionary<string, object>();

            foreach (var pair in changes)
            {
                changedProperties.Add(pair.Key, pair.Value.ToObject(ConvertToType<T>(pair.Key)));
            }

            _unitOfWork.GetRepository<T>().UpdateChangedProperties(id, changedProperties);

            T t = _unitOfWork.GetRepository<T>().GetById(id);

            return GetIDataResponse(() => t);
        }
        public IDataResponse Add(T entity)
        {
            var response = new DataResponse();
            try
            {
                _unitOfWork.GetRepository<T>().Insert(entity);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
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
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
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
                var dataResponse = new DataResponse<T1>
                {
                    Data = result,
                    IsSuccessful = true
                };
                return dataResponse;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
                var response = new DataResponse<T1> { IsSuccessful = false };
                response.ErrorMessages.Add(e.Message);
                response.VerboseErrorMessages.Add(e.ToString());
                return response;
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
                _logger.Error(ex.Message, ex);
                response = new DataResponse { IsSuccessful = false };
                response.ErrorMessages.Add(ex.Message);
                response.VerboseErrorMessages.Add(ex.ToString());
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

        public static IDataResponse SafeExecute(Action method)
        {
            var response = new DataResponse();
            try
            {
                method.Invoke();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
                _logger.Error(ex);
            }
            return response;
        }

        public static IDataResponse<T1> SafeExecute<T1>(Func<T1> method)
        {
            var response = new DataResponse<T1>();
            try
            {
                response.Data = method.Invoke();
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
                _logger.Error(ex);
            }
            return response;
        }
    }
}

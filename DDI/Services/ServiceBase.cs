﻿using DDI.Data;
using DDI.Shared;
using DDI.Shared.Logger;
using DDI.Shared.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using DDI.Services.Search;
using DDI.Shared.Statics;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Services
{
    public class ServiceBase<T> : IService<T> where T : class, IEntity
    {
        private static readonly Logger _logger = Logger.GetLogger(typeof(ServiceBase<T>));
        private readonly IUnitOfWork _unitOfWork;
        private Expression<Func<T, object>>[] _includesForSingle = null;
        private Expression<Func<T, object>>[] _includesForList = null;

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

        public Expression<Func<T, object>>[] IncludesForSingle
        {
            protected get { return _includesForSingle; }
            set { _includesForSingle = value; }
        }

        public Expression<Func<T, object>>[] IncludesForList
        {
            protected get { return _includesForList; }
            set { _includesForList = value; }
        }

        public virtual IDataResponse<List<T>> GetAll(IPageable search = null)
        {
            var queryable = _unitOfWork.GetRepository<T>().GetEntities(_includesForList);
            return GetPagedResults(queryable, search);
        }

        private IDataResponse<List<T>> GetPagedResults(IQueryable<T> queryable, IPageable search = null)
        {
            if (search == null)
            {
                search = new PageableSearch
                {
                    Limit = SearchParameters.LimitDefault,
                    Offset = SearchParameters.OffsetDefault
                };
            }

            var query = new CriteriaQuery<T, IPageable>(queryable, search);

            if (!string.IsNullOrWhiteSpace(search.OrderBy) && search.OrderBy != OrderByProperties.DisplayName)
            {
                query = query.SetOrderBy(search.OrderBy);
            }

            var totalCount = query.GetQueryable().Count();

            query = query.SetLimit(search.Limit)
                         .SetOffset(search.Offset);

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            var response = GetIDataResponse(() => query.GetQueryable().ToList());
            if (search.OrderBy == OrderByProperties.DisplayName)
            {
                response.Data = response.Data.OrderBy(a => a.DisplayName).ToList();
            }
            response.Data = ModifySortOrder(response.Data);

            response.TotalResults = totalCount;

            return response;
        }

        protected virtual List<T> ModifySortOrder(List<T> data)
        {
            return data;
        }

        public virtual IDataResponse<T> GetById(Guid id)
        {
            var result = _unitOfWork.GetRepository<T>().GetById(id, _includesForSingle); 
            return GetIDataResponse(() => result);
        }

        public IDataResponse<List<T>> GetAllWhereExpression(Expression<Func<T, bool>> expression, IPageable search = null)
        {
            var queryable = UnitOfWork.GetRepository<T>().GetEntities(_includesForList).Where(expression);
            return GetPagedResults(queryable, search);
        }

        public virtual IDataResponse Update(T entity)
        {
            var response = new DataResponse<T>();
            try
            {
                _unitOfWork.GetRepository<T>().Update(entity);
                _unitOfWork.SaveChanges();
                response.Data = _unitOfWork.GetRepository<T>().GetById(entity.Id, IncludesForSingle);
            }
            catch (Exception ex)
            {
                return ProcessIDataResponseException(ex);
            }

            return response;
        }

        public virtual IDataResponse<T> Update(Guid id, JObject changes)
        {
            var response = new DataResponse<T>();
            Dictionary<string, object> changedProperties = new Dictionary<string, object>();
            try
            {
                foreach (var pair in changes)
                {
                    var convertedPair = JsonExtensions.ConvertToType<T>(pair);
                    changedProperties.Add(convertedPair.Key, convertedPair.Value);
                }

                _unitOfWork.GetRepository<T>().UpdateChangedProperties(id, changedProperties);
            	_unitOfWork.SaveChanges();

                response.Data = _unitOfWork.GetRepository<T>().GetById(id, IncludesForSingle);
            }
            catch (Exception ex)
            {
                return ProcessIDataResponseException(ex);
            }

            return response;
        }

        public virtual IDataResponse<T> Add(T entity)
        {
            var response = new DataResponse<T>();
            try
            {
                _unitOfWork.GetRepository<T>().Insert(entity);
                _unitOfWork.SaveChanges();
                response.Data = _unitOfWork.GetRepository<T>().GetById(entity.Id);
            }
            catch (Exception ex)
            {
                return ProcessIDataResponseException(ex);
            }

            return response;
        }

        public virtual IDataResponse Delete(T entity)
        {
            var response = new DataResponse();
            try
            {
                _unitOfWork.GetRepository<T>().Delete(entity);
                _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                return ProcessIDataResponseException(ex);
            }

            return response;
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

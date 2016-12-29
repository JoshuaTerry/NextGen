﻿using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Data;
using DDI.Data.Models;
using DDI.Shared;
using DDI.WebApi.Services.Search;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Services
{
    public class GenericServiceCommonBase<T> : ServiceBase, IGenericService<T>
        where T : class, IEntity
    {
        #region Private Fields

        private IRepository<T> _repository;
        public IRepository<T> Repository => _repository;

        #endregion Private Fields

        #region Public Constructors

        public GenericServiceCommonBase()
            : this(new Repository<T>(new CommonContext()))
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal GenericServiceCommonBase(IRepository<T> repository)
        {
            _repository = repository;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IDataResponse<List<T>> GetAll(IPageable search= null)
        {
            var result = _repository.Entities.ToList().OrderBy(a => a.DisplayName).ToList();
            return GetIDataResponse(() => result);
        }

        public IDataResponse Update(T entity)
        {
            var response = new DataResponse();
            try
            {
                _repository.Update(entity);
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

            _repository.UpdateChangedProperties(id, changedProperties);

            T t = _repository.GetById(id);

            return GetIDataResponse(() => t);
        }
        public IDataResponse Add(T entity)
        {
            var response = new DataResponse();
            try
            {
                _repository.Insert(entity);
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
                _repository.Delete(entity);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        private Type ConvertToType<T>(string property)
        {
            Type classType = typeof(T);

            var propertyType = classType.GetProperty(property).PropertyType;

            return propertyType;
        }
        #endregion Public Methods
    }
}
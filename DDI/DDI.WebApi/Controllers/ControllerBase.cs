using DDI.Logger;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    public abstract class ControllerBase<T> : ApiController where T : class, IEntity
    {
        private readonly DynamicTransmogrifier _dynamicTransmogrifier;
        private readonly IService<T> _service;
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(ControllerBase<T>));
        private PathHelper.FieldListBuilder<T> _fieldListBuilder = null;

        internal IService<T> Service => _service;
        protected ILogger Logger => _logger;
        protected DynamicTransmogrifier DynamicTransmogrifier => _dynamicTransmogrifier;
        protected PathHelper.FieldListBuilder<T> FieldListBuilder => _fieldListBuilder?.Clear() ?? (_fieldListBuilder = new PathHelper.FieldListBuilder<T>());

        protected virtual string FieldsForList => string.Empty;
        protected virtual string FieldsForSingle => FieldLists.AllFields;
        protected virtual string FieldsForAll => string.Empty;

        #region Constructors 

        public ControllerBase(IService<T> serviceBase)
        {
            _dynamicTransmogrifier = new DynamicTransmogrifier();
            _service = serviceBase;
            _service.IncludesForSingle = GetDataIncludesForSingle();
            _service.IncludesForList = GetDataIncludesForList();
        }

        #endregion

        #region Methods 

        protected virtual Expression<Func<T, object>>[] GetDataIncludesForSingle()
        {
            //Each controller should implement this if they need specific children populated
            return null;
        }

        protected virtual Expression<Func<T, object>>[] GetDataIncludesForList()
        {
            //Each controller should implement this if they need specific children populated
            return null;
        }

        /// <summary>
        /// Convert a comma delimited list of fields for GET.  "all" specifies all fields, blank or null specifies default fields.
        /// </summary>
        /// <param name="fields">List of fields from API call.</param>
        /// <param name="defaultFields">Default fields list.</param>
        protected virtual string ConvertFieldList(string fields, string defaultFields = "")
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                fields = defaultFields;
            }
            if (string.Compare(fields, FieldLists.AllFields, true) == 0)
            {
                fields = FieldsForAll;
            }

            return fields;
        }

        public virtual IHttpActionResult GetById(Guid id, string fields = null)
        {
            try
            {
                var response = _service.GetById(id);
                if (!response.IsSuccessful)
                {
                    throw new Exception(string.Join(", ", response.ErrorMessages));
                }
                return FinalizeResponse(response, ConvertFieldList(fields, FieldsForSingle));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        protected IHttpActionResult FinalizeResponse<T1>(IDataResponse<List<T1>> response, IPageable search, string fields = null)
            where T1 : class
        {
            try
            {
                if (search == null)
                {
                    search = PageableSearch.Default;
                }

                if (!response.IsSuccessful)
                {
                    return BadRequest(string.Join(", ", response.ErrorMessages));
                }

                var totalCount = response.TotalResults;

                var dynamicResponse = _dynamicTransmogrifier.ToDynamicResponse(response, fields);
                if (!dynamicResponse.IsSuccessful)
                {
                    throw new Exception(string.Join(", ", dynamicResponse.ErrorMessages));
                }
                return Ok(dynamicResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        protected IHttpActionResult FinalizeResponse(IDataResponse<T> response, string fields = null)
        {
            try
            {
                if (response.Data == null)
                {
                    if (response.ErrorMessages.Count > 0)
                        return BadRequest(string.Join(",", response.ErrorMessages));
                    else
                        return NotFound();
                }
                if (!response.IsSuccessful)
                {
                    return BadRequest(string.Join(",", response.ErrorMessages));
                }

                var dynamicResponse = _dynamicTransmogrifier.ToDynamicResponse(response, fields);
                if (!dynamicResponse.IsSuccessful)
                {
                    throw new Exception(string.Join(", ", dynamicResponse.ErrorMessages));
                }

                return Ok(dynamicResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        public virtual IHttpActionResult Post(T entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception(messages.ModelInvalid);
                }


                var response = _service.Add(entity);

                if (!response.IsSuccessful)
                    throw new Exception(string.Join(",", response.ErrorMessages));

                return FinalizeResponse(response, string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        public virtual IHttpActionResult Patch(Guid id, JObject changes)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Update(id, changes);

                if (!response.IsSuccessful)
                    throw new Exception(string.Join(",", response.ErrorMessages));

                return FinalizeResponse(response, string.Empty);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        public virtual IHttpActionResult Delete(Guid id)
        {
            try
            {
                var entity = _service.GetById(id);

                if (entity.Data == null)
                {
                    return NotFound();
                }

                if (!entity.IsSuccessful)
                {
                    return BadRequest(string.Join(",", entity.ErrorMessages));
                }

                var response = _service.Delete(entity.Data);
                if (!response.IsSuccessful)
                {
                    return BadRequest(string.Join(", ", response.ErrorMessages));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        /// <summary>
        /// Invoke a custom action that returns an IDataReponse
        /// </summary>
        /// <param name="action">Action to be invoked.</param>
        protected virtual IHttpActionResult CustomAction<T1>(Func<IDataResponse<T1>> action)
        {
            try
            {
                IDataResponse<T1> result = action();
                if (result.Data == null)
                {
                    return NotFound();
                }

                if (!result.IsSuccessful)
                {
                    return BadRequest(string.Join(",", result.ErrorMessages));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        #endregion

    }
}
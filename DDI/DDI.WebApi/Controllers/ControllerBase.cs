using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Http.Routing;
using DDI.Logger;
using DDI.Services;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using DDI.WebApi.Helpers;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public abstract class ControllerBase<T> : ApiController where T : class, IEntity
    {
        private readonly IPagination _pagination;
        private readonly DynamicTransmogrifier _dynamicTransmogrifier;
        private readonly IService<T> _service;
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(ControllerBase<T>));
        private PathHelper.FieldListBuilder<T> _fieldListBuilder = null;

        protected IService<T> Service => _service;
        protected ILogger Logger => _logger;
        protected DynamicTransmogrifier DynamicTransmogrifier => _dynamicTransmogrifier;
        protected IPagination Pagination => _pagination;
        protected PathHelper.FieldListBuilder<T> FieldListBuilder => _fieldListBuilder ?? (_fieldListBuilder = new PathHelper.FieldListBuilder<T>());

        protected virtual string FieldsForList => string.Empty;
        protected virtual string FieldsForSingle => "all";
        protected virtual string FieldsForAll => string.Empty;

        #region Constructors 

        public ControllerBase()
            :this(new ServiceBase<T>())
        {
        }

        public ControllerBase(IService<T> serviceBase)
            : this(serviceBase, new DynamicTransmogrifier(), new Pagination())
        {
        }

        internal ControllerBase(IService<T> serviceBase, DynamicTransmogrifier dynamicTransmogrifier, IPagination pagination)
        {
            _pagination = pagination;
            _dynamicTransmogrifier = dynamicTransmogrifier;
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
            if (string.Compare(fields, "all", true) == 0)
            {
                fields = FieldsForAll;
            }

            return fields;
        }



        protected UrlHelper GetUrlHelper()
        {
            var urlHelper = new UrlHelper(Request);
            return urlHelper;
        }
          
        protected IHttpActionResult GetById(Guid id, string fields = null, UrlHelper urlHelper = null)
        {
            try
            {                
                urlHelper = urlHelper ?? GetUrlHelper();
                var response = _service.GetById(id);
                if (!response.IsSuccessful)
                {
                    throw new Exception(string.Join(", ", response.ErrorMessages));
                }
                return FinalizeResponse(response, ConvertFieldList(fields, FieldsForSingle), urlHelper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        protected IHttpActionResult FinalizeResponse<T1>(IDataResponse<List<T1>> response, string routeName, IPageable search, string fields = null, UrlHelper urlHelper = null)
            where T1 : class
        {
            try
            {
                if (search == null)
                {
                    search = PageableSearch.Default;
                }

                urlHelper = urlHelper ?? GetUrlHelper();
                if (!response.IsSuccessful)
                {
                    return BadRequest(string.Join(", ", response.ErrorMessages));
                }

                var totalCount = response.TotalResults;

                _pagination.AddPaginationHeaderToResponse(urlHelper, search, totalCount, routeName);
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

        protected IHttpActionResult FinalizeResponse(IDataResponse<T> response, string fields = null, UrlHelper urlHelper = null)
        {
            try
            {
                urlHelper = urlHelper ?? GetUrlHelper();
                if (response.Data == null)
                {
                    if (response.ErrorMessages.Count > 0)
                        return  BadRequest(string.Join(",", response.ErrorMessages));
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

        protected IHttpActionResult Post(T entity, UrlHelper urlHelper = null)
        {
            try
            {
                urlHelper = urlHelper ?? GetUrlHelper();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Add(entity);

                if (!response.IsSuccessful)
                    throw new Exception(string.Join(",", response.ErrorMessages));

                return FinalizeResponse(response, string.Empty, urlHelper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        protected IHttpActionResult Patch(Guid id, JObject changes, UrlHelper urlHelper = null)
        {
            try
            {
                urlHelper = urlHelper ?? GetUrlHelper();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Update(id, changes);

                if (!response.IsSuccessful)
                    throw new Exception(string.Join(",", response.ErrorMessages));

                return FinalizeResponse(response, string.Empty, urlHelper);

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
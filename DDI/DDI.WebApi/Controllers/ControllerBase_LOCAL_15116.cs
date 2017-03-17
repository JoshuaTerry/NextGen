﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using DDI.Services;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;

using DDI.Shared.Models;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using Newtonsoft.Json.Linq;
using System.Web.Http.Results;
using DDI.Logger;

namespace DDI.WebApi.Controllers
{
    public class ControllerBase<T> : ApiController
        where T : class, IEntity
    {
        private IPagination _pagination;
        private DynamicTransmogrifier _dynamicTransmogrifier;
        private readonly IService<T> _service;
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(ControllerBase<T>));
        protected IService<T> Service => _service;
        protected ILogger Logger => _logger;

        protected virtual string FieldsForList => string.Empty;
        protected virtual string FieldsForSingle => "all";
        protected virtual string FieldsForAll => string.Empty;

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


        public IPagination Pagination
        {
            get { return _pagination; }
            set { _pagination = value; }
        }

        public DynamicTransmogrifier DynamicTransmogrifier
        {
            get { return _dynamicTransmogrifier; }
            set { _dynamicTransmogrifier = value; }
        }

        protected UrlHelper GetUrlHelper()
        {
            var urlHelper = new UrlHelper(Request);
            return urlHelper;
        }

        public IHttpActionResult GetAll(string routeName, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null, UrlHelper urlHelper = null)
        {
            var search = new PageableSearch()
            {
                Limit = limit,
                Offset = offset,
                OrderBy = orderBy
            };

            return GetAll(routeName, search, fields, urlHelper ?? GetUrlHelper());

        }

        public IHttpActionResult GetAll(string routeName, IPageable search, string fields = null, UrlHelper urlHelper = null)
        {
            try
            {                
                urlHelper = urlHelper ?? GetUrlHelper();
                return FinalizeResponse(_service.GetAll(fields, search), routeName, search, ConvertFieldList(fields, FieldsForList), urlHelper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError();
            }

        }

        public IHttpActionResult GetById(Guid id, string fields = null, UrlHelper urlHelper = null)
        {
            try
            {                
                urlHelper = urlHelper ?? GetUrlHelper();
                var response = _service.GetById(id);
                return FinalizeResponse(response, ConvertFieldList(fields, FieldsForSingle), urlHelper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError();
            }
        }

        public IHttpActionResult FinalizeResponse<T1>(IDataResponse<List<T1>> response, string routeName, IPageable search, string fields = null, UrlHelper urlHelper = null)
            where T1 : class
        {
            try
            {
                urlHelper = urlHelper ?? GetUrlHelper();
                if (!response.IsSuccessful)
                {
                    return BadRequest(response.ErrorMessages.ToString());
                }

                var totalCount = response.TotalResults;

                Pagination.AddPaginationHeaderToResponse(urlHelper, search, totalCount, routeName);
                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, fields);

                return Ok(dynamicResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError();
            }
        }

        public IHttpActionResult FinalizeResponse(IDataResponse<T> response, string fields = null, UrlHelper urlHelper = null)
        {
            try
            {
                urlHelper = urlHelper ?? GetUrlHelper();
                if (response.Data == null)
                {
                    return NotFound();
                }
                if (!response.IsSuccessful)
                {
                    return BadRequest(response.ErrorMessages.ToString());
                }

                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, fields);

                return Ok(dynamicResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError();
            }
        }

        public IHttpActionResult Post(T entity, UrlHelper urlHelper = null)
        {
            try
            {
                urlHelper = urlHelper ?? GetUrlHelper();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Add(entity);
                return FinalizeResponse(response, string.Empty, urlHelper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError();
            }
        }

        public IHttpActionResult Patch(Guid id, JObject changes, UrlHelper urlHelper = null)
        {
            try
            {
                urlHelper = urlHelper ?? GetUrlHelper();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Update(id, changes);
                return FinalizeResponse(response, string.Empty, urlHelper);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError();
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
                    return BadRequest(entity.ErrorMessages.ToString());
                }

                var response = _service.Delete(entity.Data);
                if (!response.IsSuccessful)
                {
                    return BadRequest(response.ErrorMessages.ToString());
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError();
            }
        }
    }
}
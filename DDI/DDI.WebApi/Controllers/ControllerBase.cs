using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Models;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class ControllerBase<T> : ApiController
        where T : class, IEntity
    {
        private IPagination _pagination;
        private DynamicTransmogrifier _dynamicTransmogrifier;
        private ServiceBase<T> _service;

        public ControllerBase()
            :this(new ServiceBase<T>(), new DynamicTransmogrifier(), new Pagination())
        {
        }

        internal ControllerBase(ServiceBase<T> serviceBase, DynamicTransmogrifier dynamicTransmogrifier, IPagination pagination)
        {
            _pagination = pagination;
            _dynamicTransmogrifier = dynamicTransmogrifier;
            _service = serviceBase;
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

        public IHttpActionResult GetAll(UrlHelper urlHelper, string routeName, int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            var search = new PageableSearch()
            {
                Limit = limit,
                Offset = offset,
                OrderBy = orderBy
            };

            try
            {
                var response = _service.GetAll(search);
                if (response.Data == null)
                {
                    return NotFound();
                }
                if (!response.IsSuccessful)
                {
                    return BadRequest(response.ErrorMessages.ToString());
                }

                var totalCount = response.TotalResults;

                Pagination.AddPaginationHeaderToResponse(urlHelper, search, totalCount, routeName);
                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, urlHelper, fields);

                return Ok(dynamicResponse);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
            
        }

        public IHttpActionResult GetById(UrlHelper urlHelper, Guid id, string fields = null)
        {
            try
            {
                var response = _service.GetById(id);
                if (response.Data == null)
                {
                    return NotFound();
                }
                if (!response.IsSuccessful)
                {
                    return BadRequest(response.ErrorMessages.ToString());
                }

                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, urlHelper, fields);

                return Ok(dynamicResponse);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Post(UrlHelper urlHelper, T entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Add(entity);
                if (!response.IsSuccessful)
                {
                    return BadRequest(response.ErrorMessages.ToString());
                }

                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, urlHelper);

                return Ok(dynamicResponse);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Patch(UrlHelper urlHelper, Guid id, JObject changes)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Update(id, changes);
                if (!response.IsSuccessful)
                {
                    return BadRequest(response.ErrorMessages.ToString());
                }

                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, urlHelper);

                return Ok(dynamicResponse);

            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        protected ServiceBase<T> Service { get { return _service; } }
    }
}
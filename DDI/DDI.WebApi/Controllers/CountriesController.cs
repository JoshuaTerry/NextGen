using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Web.Http;
using DDI.Shared.Models.Common;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq; 

namespace DDI.WebApi.Controllers
{
    public class CountriesController : ControllerBase
    {
        #region Private Fields

        private ServiceBase<Country> _service;

        #endregion Private Fields

        #region Public Constructors

        public CountriesController()
            : this(new ServiceBase<Country>())
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal CountriesController(ServiceBase<Country> service)
        {
            _service = service;
        }

        #endregion Internal Constructors

        #region Public Methods

        [HttpGet]
        [Route("api/v1/countries", Name = RouteNames.Country)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderby = "Description", string fields = null)
        {
            var search = new PageableSearch()
            {
                Limit = limit,
                Offset = offset,
                OrderBy = orderby
            };

            try
            {
                var result = _service.GetAll(search);

                if (result == null)
                {
                    return NotFound();
                }
                if (!result.IsSuccessful)
                {
                    return InternalServerError();
                }

                var totalCount = result.TotalResults;

                Pagination.AddPaginationHeaderToResponse(GetUrlHelper(), search, totalCount, RouteNames.Country);

                return Ok(result);

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/v1/countries", Name = RouteNames.Country + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Country item)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Add(item);

                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, GetUrlHelper());

                return Ok(dynamicResponse);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPatch]
        [Route("api/v1/countries/{id}", Name = RouteNames.Country + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Update(id, changes);

                var dynmaicResponse = DynamicTransmogrifier.ToDynamicResponse(response, GetUrlHelper());

                return Ok(dynmaicResponse);

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        #endregion Public Methods
    }
}

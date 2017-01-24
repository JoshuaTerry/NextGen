using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class ConstituentTypesController : ControllerBase
    {
        ServiceBase<ConstituentType> _service;

        public ConstituentTypesController() : this(new ServiceBase<ConstituentType>()) { }
        internal ConstituentTypesController(ServiceBase<ConstituentType> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/constituenttypes", Name = RouteNames.ConstituentType)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderby = "DisplayName", string fields = null)
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

                Pagination.AddPaginationHeaderToResponse(GetUrlHelper(), search, totalCount, RouteNames.ConstituentType);
                var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper(), fields);

                return Ok(dynamicResult);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/v1/constituenttypes", Name = RouteNames.ConstituentType + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ConstituentType item)
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
        [Route("api/v1/constituenttypes/{id}", Name = RouteNames.ConstituentType + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Update(id, changes);

                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, GetUrlHelper());

                return Ok(dynamicResponse);

            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
    }
}
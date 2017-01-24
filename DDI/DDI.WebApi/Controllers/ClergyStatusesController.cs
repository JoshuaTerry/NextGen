using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class ClergyStatusesController : ControllerBase
    {
        ServiceBase<ClergyStatus> _service;

        public ClergyStatusesController() : this(new ServiceBase<ClergyStatus>()) { }
        internal ClergyStatusesController(ServiceBase<ClergyStatus> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/clergystatuses")]
        public IHttpActionResult GetAll(int? limit = 25, int? offset = 0, string orderby = null, string fields = null)
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

                var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper(), fields);

                return Ok(dynamicResult);

            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/v1/clergystatuses")]
        public IHttpActionResult Post([FromBody] ClergyStatus item)
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
        [Route("api/v1/clergystatuses/{id}")]
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
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
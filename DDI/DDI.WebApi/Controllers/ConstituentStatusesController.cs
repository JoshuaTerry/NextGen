using System;
using System.Web.Http;
using DDI.Data.Models.Client;
using DDI.WebApi.Services;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class ConstituentStatusesController : ApiController
    {
        GenericServiceBase<ConstituentStatus> _service;

        public ConstituentStatusesController() : this(new GenericServiceBase<ConstituentStatus>()) { }
        internal ConstituentStatusesController(GenericServiceBase<ConstituentStatus> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/constituentstatues")]
        public IHttpActionResult GetAll()
        {
            var result = _service.GetAll();

            if (result == null)
            {
                return NotFound();
            }
            if (!result.IsSuccessful)
            {
                return InternalServerError();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("api/v1/constituentstatuses")]
        public IHttpActionResult Post([FromBody] ConstituentStatus item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.Add(item);
            return Ok();
        }

        [HttpPatch]
        [Route("api/v1/constituentstatuses/{id}")]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = _service.Update(id, changes);

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
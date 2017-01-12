using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class ClergyTypesController : ApiController
    {
        ServiceBase<ClergyType> _service;

        public ClergyTypesController() : this(new ServiceBase<ClergyType>()) { }
        internal ClergyTypesController(ServiceBase<ClergyType> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/clergytypes")]
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
        [Route("api/v1/clergytypes")]
        public IHttpActionResult Post([FromBody] ClergyType item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.Add(item);
            return Ok();
        }

        [HttpPatch]
        [Route("api/v1/clergytypes/{id}")]
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
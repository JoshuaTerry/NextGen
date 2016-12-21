using DDI.Business.Services;
using DDI.Business.Services.Search;
using DDI.Data.Models.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;


namespace DDI.Business.Controllers
{
    public class ConstituentTypesController : ApiController
    {
        GenericServiceBase<ConstituentType> _service;

        public ConstituentTypesController() : this(new GenericServiceBase<ConstituentType>()) { }
        internal ConstituentTypesController(GenericServiceBase<ConstituentType> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/constituenttypes")]
        public IHttpActionResult GetAll(string orderBy = "Name")
        {
            var result = _service.GetAll(new PageableSearch { OrderBy = orderBy });

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
        [Route("api/v1/constituenttypes")]
        public IHttpActionResult Post([FromBody] ConstituentType item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = _service.Add(item);
            return Ok();
        }

        [HttpPatch]
        [Route("api/v1/constituenttypes/{id}")]
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
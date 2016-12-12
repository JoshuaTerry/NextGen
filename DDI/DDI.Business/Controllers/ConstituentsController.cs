using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DDI.Business.Services;

namespace DDI.Business.Controllers
{
    //[Authorize]
    public class ConstituentsController : ApiController
    {
        private IConstituentService _service;

        public ConstituentsController()
            :this(new ConstituentService())
        {
        }

        internal ConstituentsController(IConstituentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/constituents")]
        public IHttpActionResult GetConstituents()
        {
            var constituents = _service.GetConstituents();

            if (constituents == null)
            {
                return NotFound();
            }
            if (!constituents.IsSuccessful)
            {
                return InternalServerError();
            } 

            return Ok(constituents);
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult GetConstituentById(Guid id)
        {
            var constituent = _service.GetConstituentById(id);

            if (constituent == null)
            {
                return NotFound();
            }
            if (!constituent.IsSuccessful)
            {
                return InternalServerError();
            }

            return Ok(constituent);
        }

        [HttpPost]
        [Route("api/v1/constituents")]
        public IHttpActionResult Post([FromBody]string value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Model");
            }

            return Ok();
        }

        [HttpPut]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Model");
            }

            return Ok();
        }

        [HttpPatch]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Patch(int id, [FromBody]string value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Model");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Delete(int id)
        {
            return Ok();
        }
    }
}

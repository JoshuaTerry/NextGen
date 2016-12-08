using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DDI.Business.Services;

namespace DDI.Business.Controllers
{
    [Authorize]
    public class ConstituentsController : ApiController
    {
        private IConstituentService _service;

        // GET api/constituents
        public IHttpActionResult GetConstituents()
        {
            var constituents = _service.GetConstituents();

            if (constituents == null)
            {
                return NotFound();
            }

            return Ok();
        }

        // GET api/constituents/5
        public IHttpActionResult GetConstituent(int id)
        {
            var constituent = _service.GetConstituents();

            if (constituent == null)
            {
                return NotFound();
            }

            return Ok();
        }

        // POST api/constituents
        public IHttpActionResult Post([FromBody]string value)
        {
            return Ok();
        }

        // PUT api/constituents/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
            return Ok();
        }

        // DELETE api/constituents/5
        public IHttpActionResult Delete(int id)
        {
            return Ok();
        }
    }
}

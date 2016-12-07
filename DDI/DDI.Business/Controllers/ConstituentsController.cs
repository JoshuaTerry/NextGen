using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DDI.Business.Controllers
{
    [Authorize]
    public class ConstituentsController : ApiController
    {
        // GET api/constituents
        public IHttpActionResult GetConstituents()
        {
            return Ok();
        }

        // GET api/constituents/5
        public IHttpActionResult GetConstituent(int id)
        {
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

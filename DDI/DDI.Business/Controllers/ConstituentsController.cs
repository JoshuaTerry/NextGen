using System; 
using System.Web.Http;
using System.Web.Http.Cors;
using DDI.Business.Services;

namespace DDI.Business.Controllers
{
    //[Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
        public IHttpActionResult GetConstituents(string quickSearch = null, 
                                                 string name = null, 
                                                 int? constituentNumber = null, 
                                                 string address = null, 
                                                 string city = null, 
                                                 string state = null, 
                                                 string zip = null, 
                                                 int? offset = null, 
                                                 int? limit = 25, 
                                                 string orderby = null)
        {
            var search = new ConstituentSearch()
            {
                QuickSearch = quickSearch,
                Name = name,
                ConstituentNumber = constituentNumber,
                Address = address,
                City = city,
                State = state,
                Zip = zip,
                Offset = offset,
                Limit = limit,
                OrderBy = orderby
            };

            var constituents = _service.GetConstituents(search);

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

        [HttpGet]
        [Route("api/v1/constituents/number/{num}")]
        public IHttpActionResult GetConstituentByConstituentNum(int num)
        {
            {
                var constituent = _service.GetConstituentByConstituentNum(num);

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
        public IHttpActionResult Put(Guid id, [FromBody]string value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Model");
            }

            return Ok();
        }

        [HttpPatch]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Patch(Guid id, [FromBody]string value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Model");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Delete(Guid id)
        {
            return Ok();
        }
    }
}

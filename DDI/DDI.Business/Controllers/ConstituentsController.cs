using System; 
using System.Web.Http;
using System.Web.Http.Cors;
using DDI.Business.Services;
using DDI.Data.Models.Client;
using Newtonsoft.Json.Linq;

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
        public IHttpActionResult GetConstituents(string quickSearch = null, 
                                                 string name = null, 
                                                 int? constituentNumber = null, 
                                                 string address = null, 
                                                 string city = null, 
                                                 string state = null,
                                                 string zipFrom = null,
                                                 string zipTo = null,
                                                 string alternateId = null,
                                                 Guid? constituentTypeId = null,
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
                Offset = offset,
                Limit = limit,
                OrderBy = orderby,
                AlternateId = alternateId,
                ZipFrom =  zipFrom,
                ZipTo = zipTo,
                ConstituentTypeId = constituentTypeId
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
        public IHttpActionResult Post([FromBody] Constituent constituent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response =_service.AddConstituent(constituent);
            return Ok(response);
        }

        [HttpPut]
        [Route("api/v1/constituents/")]
        public IHttpActionResult Put(Constituent constituentChanges)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //var updatedConstituent = _service.UpdateConstituent(constituentChanges);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Patch(Guid id, JObject constituentChanges)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _service.UpdateConstituent(id, constituentChanges);

            return Ok();
        }

        [HttpDelete]
        [Route("api/v1/constituents/{id}")]
        public IHttpActionResult Delete(Guid id)
        {
            return Ok();
        }

        [HttpGet]
        [Route("api/v1/constituents/{id}/dbas")]
        public IHttpActionResult GetConstituentDBAs(Guid constituentId)
        {
            var result = _service.GetConstituentDBAs(constituentId);

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

        [HttpGet]
        [Route("api/v1/constituents/{id}/educationlevel")]
        public IHttpActionResult GetEducationLevels(Guid constituentId)
        {
            var result = _service.GetEducationLevels(constituentId);

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
    }
}

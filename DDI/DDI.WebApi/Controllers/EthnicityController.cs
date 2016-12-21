using System.Web.Http;
using DDI.Data.Models.Client;
using DDI.WebApi.Services;

namespace DDI.WebApi.Controllers
{
    public class EthnicityController : ApiController
    {
        GenericServiceBase<Ethnicity> _service;

        public EthnicityController() : this(new GenericServiceBase<Ethnicity>()) { }
        internal EthnicityController(GenericServiceBase<Ethnicity> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/ethnicity")]
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
    }
}
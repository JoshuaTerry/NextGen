using DDI.Business.Services;
using DDI.Data.Models.Client;
using System.Web.Http;
using DDI.Business.Services.Search;

namespace DDI.Business.Controllers
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
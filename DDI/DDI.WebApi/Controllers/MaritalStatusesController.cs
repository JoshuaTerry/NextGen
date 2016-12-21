using System.Web.Http;
using DDI.Data.Models.Client;
using DDI.WebApi.Services;

namespace DDI.WebApi.Controllers
{
    public class MaritalStatusesController : ApiController
    {
        GenericServiceBase<MaritalStatus> _service;

        public MaritalStatusesController() : this(new GenericServiceBase<MaritalStatus>()) { }
        internal MaritalStatusesController(GenericServiceBase<MaritalStatus> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/maritalstatuses")]
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
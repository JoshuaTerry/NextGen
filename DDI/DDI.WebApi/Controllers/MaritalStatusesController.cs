using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;

namespace DDI.WebApi.Controllers
{
    public class MaritalStatusesController : ApiController
    {
        ServiceBase<MaritalStatus> _service;

        public MaritalStatusesController() : this(new ServiceBase<MaritalStatus>()) { }
        internal MaritalStatusesController(ServiceBase<MaritalStatus> service)
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
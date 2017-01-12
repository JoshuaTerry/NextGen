using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;

namespace DDI.WebApi.Controllers
{
    public class EthnicityController : ApiController
    {
        ServiceBase<Ethnicity> _service;

        public EthnicityController() : this(new ServiceBase<Ethnicity>()) { }
        internal EthnicityController(ServiceBase<Ethnicity> service)
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
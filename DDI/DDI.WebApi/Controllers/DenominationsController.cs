using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;

namespace DDI.WebApi.Controllers
{
    public class DenominationsController : ApiController
    {
        ServiceBase<Denomination> _service;

        public DenominationsController() : this(new ServiceBase<Denomination>()) { }
        internal DenominationsController(ServiceBase<Denomination> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/denominations")]
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
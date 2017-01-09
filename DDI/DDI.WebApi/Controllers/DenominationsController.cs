using System.Web.Http;
using DDI.Data.Models.Client.CRM;
using DDI.WebApi.Services;

namespace DDI.WebApi.Controllers
{
    public class DenominationsController : ApiController
    {
        GenericServiceBase<Denomination> _service;

        public DenominationsController() : this(new GenericServiceBase<Denomination>()) { }
        internal DenominationsController(GenericServiceBase<Denomination> service)
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
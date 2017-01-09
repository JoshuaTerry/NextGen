using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;

namespace DDI.WebApi.Controllers
{
    public class PrefixesController : ApiController
    {
        ServiceBase<Prefix> _service;

        public PrefixesController() : this(new ServiceBase<Prefix>()) { }
        internal PrefixesController(ServiceBase<Prefix> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/prefixes")]
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
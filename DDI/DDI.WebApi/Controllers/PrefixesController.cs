using System.Web.Http;
using DDI.Data.Models.Client.CRM;
using DDI.WebApi.Services;

namespace DDI.WebApi.Controllers
{
    public class PrefixesController : ApiController
    {
        GenericServiceBase<Prefix> _service;

        public PrefixesController() : this(new GenericServiceBase<Prefix>()) { }
        internal PrefixesController(GenericServiceBase<Prefix> service)
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
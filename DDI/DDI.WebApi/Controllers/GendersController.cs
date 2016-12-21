using System.Web.Http;
using DDI.Data.Models.Client;
using DDI.WebApi.Services;

namespace DDI.WebApi.Controllers
{
    public class GendersController : ApiController
    {
        GenericServiceBase<Gender> _service;

        public GendersController() : this(new GenericServiceBase<Gender>()) { }
        internal GendersController(GenericServiceBase<Gender> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/genders")]
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
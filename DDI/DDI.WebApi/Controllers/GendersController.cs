using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;

namespace DDI.WebApi.Controllers
{
    public class GendersController : ApiController
    {
        ServiceBase<Gender> _service;

        public GendersController() : this(new ServiceBase<Gender>()) { }
        internal GendersController(ServiceBase<Gender> service)
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
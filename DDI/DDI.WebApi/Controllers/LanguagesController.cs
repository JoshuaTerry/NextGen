using System.Web.Http;
using DDI.Services;
using DDI.Shared.Models.Client.Core; 

namespace DDI.WebApi.Controllers
{
    public class LanguagesController : ApiController
    {
        ServiceBase<Language> _service;

        public LanguagesController() : this(new ServiceBase<Language>()) { }
        internal LanguagesController(ServiceBase<Language> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/languages")]
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
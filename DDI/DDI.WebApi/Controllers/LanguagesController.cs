using System.Web.Http;
using DDI.Data.Models.Client;
using DDI.WebApi.Services;

namespace DDI.WebApi.Controllers
{
    public class LanguagesController : ApiController
    {
        GenericServiceBase<Language> _service;

        public LanguagesController() : this(new GenericServiceBase<Language>()) { }
        internal LanguagesController(GenericServiceBase<Language> service)
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
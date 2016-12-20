using DDI.Business.Services;
using DDI.Data.Models.Client;
using System.Web.Http;
using DDI.Business.Services.Search;

namespace DDI.Business.Controllers
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
        public IHttpActionResult GetAll(string orderBy = "Name")
        {
            var result = _service.GetAll(new PageableSearch { OrderBy = orderBy });

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
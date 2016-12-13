using DDI.Business.Services;
using DDI.Data.Models.Client;
using System.Web.Http;

namespace DDI.Business.Controllers
{
    public class ProfessionsController : ApiController
    {
        GenericServiceBase<Profession> _service;

        public ProfessionsController() : this(new GenericServiceBase<Profession>()) { }
        internal ProfessionsController(GenericServiceBase<Profession> service)
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
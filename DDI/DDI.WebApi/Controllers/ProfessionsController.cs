using System.Web.Http;
using DDI.Data.Models.Client;
using DDI.WebApi.Services;

namespace DDI.WebApi.Controllers
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
        [Route("api/v1/professions")]
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
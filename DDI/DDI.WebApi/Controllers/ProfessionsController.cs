using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    public class ProfessionsController : ApiController
    {
        ServiceBase<Profession> _service;

        public ProfessionsController() : this(new ServiceBase<Profession>()) { }
        internal ProfessionsController(ServiceBase<Profession> service)
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
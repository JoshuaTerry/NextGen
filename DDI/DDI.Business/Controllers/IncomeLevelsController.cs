using DDI.Business.Services;
using DDI.Data.Models.Client;
using System.Web.Http;

namespace DDI.Business.Controllers
{
    public class IncomeLevelsController : ApiController
    {
        GenericServiceBase<IncomeLevel> _service;

        public IncomeLevelsController() : this(new GenericServiceBase<IncomeLevel>()) { }
        internal IncomeLevelsController(GenericServiceBase<IncomeLevel> service)
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
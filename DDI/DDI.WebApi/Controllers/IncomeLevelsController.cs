using System.Web.Http;
using DDI.Data.Models.Client;
using DDI.WebApi.Services;

namespace DDI.WebApi.Controllers
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
        [Route("api/v1/incomelevels")]
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
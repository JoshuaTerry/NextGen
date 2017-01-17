using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;

namespace DDI.WebApi.Controllers
{
    public class IncomeLevelsController : ApiController
    {
        ServiceBase<IncomeLevel> _service;

        public IncomeLevelsController() : this(new ServiceBase<IncomeLevel>()) { }
        internal IncomeLevelsController(ServiceBase<IncomeLevel> service)
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
using DDI.Business.Services;
using DDI.Business.Services.Search;
using DDI.Data.Models.Client;
using System.Web.Http;

namespace DDI.Business.Controllers
{
    public class DenominationsController : ApiController
    {
        GenericServiceBase<Denomination> _service;

        public DenominationsController() : this(new GenericServiceBase<Denomination>()) { }
        internal DenominationsController(GenericServiceBase<Denomination> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/denominations")]
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
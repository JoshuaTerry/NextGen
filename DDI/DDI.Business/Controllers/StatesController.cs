using System.Web.Http;
using DDI.Business.Services;
using DDI.Data.Models.Common;

namespace DDI.Business.Controllers
{
    public class StatesController : ApiController
    {
        GenericServiceBase<State> _service;

        public StatesController() : this(new GenericServiceBase<State>()) { }
        internal StatesController(GenericServiceBase<State> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/countries")]
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
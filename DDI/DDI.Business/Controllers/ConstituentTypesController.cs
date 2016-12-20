using DDI.Business.Services;
using DDI.Business.Services.Search;
using DDI.Data.Models.Client;
using System.Web.Http;


namespace DDI.Business.Controllers
{
    public class ConstituentTypesController : ApiController
    {
        GenericServiceBase<ConstituentType> _service;

        public ConstituentTypesController() : this(new GenericServiceBase<ConstituentType>()) { }
        internal ConstituentTypesController(GenericServiceBase<ConstituentType> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/constituenttypes")]
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
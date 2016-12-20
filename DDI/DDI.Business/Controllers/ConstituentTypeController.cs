using System.Web.Http;
using System.Web.Http.Cors;
using DDI.Business.Services;
using DDI.Business.Services.Search;
using DDI.Data.Models.Client;

namespace DDI.Business.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConstituentTypeController : ApiController
    {
        GenericServiceBase<ConstituentType> _service;

        public ConstituentTypeController() : this(new GenericServiceBase<ConstituentType>()) { }
        internal ConstituentTypeController(GenericServiceBase<ConstituentType> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/constituenttypes")]
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
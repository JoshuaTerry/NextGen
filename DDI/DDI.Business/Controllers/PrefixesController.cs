using DDI.Business.Services;
using DDI.Data.Models.Client;
using System.Web.Http;
using DDI.Business.Services.Search;

namespace DDI.Business.Controllers
{
    public class PrefixesController : ApiController
    {
        GenericServiceBase<Prefix> _service;

        public PrefixesController() : this(new GenericServiceBase<Prefix>()) { }
        internal PrefixesController(GenericServiceBase<Prefix> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/prefixes")]
        public IHttpActionResult GetAll(string orderBy = "Abbreviation")
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
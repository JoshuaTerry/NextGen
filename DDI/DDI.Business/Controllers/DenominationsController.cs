using System.Web.Http;
using DDI.Business.Services;
using DDI.Business.Services.Search;
using DDI.Data.Models.Client;

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
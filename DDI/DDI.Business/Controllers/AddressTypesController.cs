using DDI.Business.Services;
using DDI.Business.Services.Search;
using DDI.Data.Models.Client;
using System.Web.Http;

namespace DDI.Business.Controllers
{
    public class AddressTypesController : ApiController
    {
        GenericServiceBase<AddressType> _service;

        public AddressTypesController() : this(new GenericServiceBase<AddressType>()) { }
        internal AddressTypesController(GenericServiceBase<AddressType> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/addresstypes")]
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
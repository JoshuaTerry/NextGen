using System.Web.Http;
using DDI.Data.Models.Client;
using DDI.WebApi.Services;

namespace DDI.WebApi.Controllers
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
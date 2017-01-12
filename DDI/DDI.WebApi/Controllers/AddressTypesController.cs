using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;

namespace DDI.WebApi.Controllers
{
    public class AddressTypesController : ApiController
    {
        ServiceBase<AddressType> _service;

        public AddressTypesController() : this(new ServiceBase<AddressType>()) { }
        internal AddressTypesController(ServiceBase<AddressType> service)
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
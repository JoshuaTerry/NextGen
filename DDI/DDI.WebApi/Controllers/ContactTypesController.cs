using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;

namespace DDI.WebApi.Controllers
{
    public class ContactTypesController : ApiController
    {
        ServiceBase<ContactType> _service;

        public ContactTypesController() : this(new ServiceBase<ContactType>()) { }
        internal ContactTypesController(ServiceBase<ContactType> service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("api/v1/contacttypes")]
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
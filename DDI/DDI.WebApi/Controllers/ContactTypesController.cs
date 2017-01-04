using System.Web.Http;
using DDI.Data.Models.Client.CRM;
using DDI.WebApi.Services;

namespace DDI.WebApi.Controllers
{
    public class ContactTypesController : ApiController
    {
        GenericServiceBase<ContactType> _service;

        public ContactTypesController() : this(new GenericServiceBase<ContactType>()) { }
        internal ContactTypesController(GenericServiceBase<ContactType> service)
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
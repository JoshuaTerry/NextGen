using System;
using System.Web.Http; 
using DDI.Business.Services;
using DDI.Business.Services.Search;
using DDI.Data.Models.Client;
using Newtonsoft.Json.Linq;


namespace DDI.Business.Controllers
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
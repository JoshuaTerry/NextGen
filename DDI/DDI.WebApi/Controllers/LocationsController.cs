using DDI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    public class LocationsController : ApiController
    {
        ILocationService _service;
        public LocationsController() : this(new LocationService()) { }

        public LocationsController(ILocationService service)
        {
            this._service = service;
        }
        
        [HttpGet]
        [Route("api/v1/locations")]
        public IHttpActionResult RefineLocation(string addressLine1, string addressLine2, string city, Guid? countryId, Guid? countyId, Guid? stateId, string zip)
        {
            var result = _service.RefineAddress(addressLine1, addressLine2, city, countryId, countyId, stateId, zip);

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
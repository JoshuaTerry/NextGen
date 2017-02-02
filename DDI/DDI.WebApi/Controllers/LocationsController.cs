using DDI.Business.Common;
using DDI.Services;
using DDI.Shared.Models.Common;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    public class LocationsController : ControllerBase<State>
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
            var fields = "City,Country,County.Id,State.Id,State.CountryId,PostalCode";

            var result = _service.RefineAddress(addressLine1, addressLine2, city, countryId, countyId, stateId, zip);

            if (result == null)
            {
                return NotFound();
            }
            if (!result.IsSuccessful)
            {
                return InternalServerError();
            }

            var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper(), fields, false);

            return Ok(dynamicResponse);
        }
    }
}
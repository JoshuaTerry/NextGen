//using DDI.Business.Common;
//using DDI.Services;
//using DDI.Shared.Models.Common;
//using System;
//using System.Web.Http;
//using System.Web.Http.Routing;
//using DDI.Shared.Logger;
//using DDI.WebApi.Helpers;
//
//namespace DDI.WebApi.Controllers
//{
//    public class LocationsController : ApiController
//    {
//        private readonly DynamicTransmogrifier _dynamicTransmogrifier;
//        private readonly Logger _logger;
//        ILocationService _service;
//        public LocationsController() : this(new LocationService(), new DynamicTransmogrifier()) { }
//
//        public LocationsController(ILocationService service, DynamicTransmogrifier dynamicTransmogrifier)
//        {
//            _service = service;
//            _dynamicTransmogrifier = dynamicTransmogrifier;
//            _logger = Logger.GetLogger(typeof(ZipLookupInfo));
//        }
//
//        [HttpGet]
//        [Route("api/v1/locations")]
//        public IHttpActionResult RefineLocation(string addressLine1 = null, string addressLine2 = null, string city = null, Guid? countryId = null, Guid? countyId = null, Guid? stateId = null, string zip = null)
//        {
//            try
//            {
//                var urlHelper = new UrlHelper(Request);
//                var fields = "City,Country,County.Id,County.Description,State.Id,State.CountryId,State.Description,PostalCode";
//
//                var response = _service.RefineAddress(addressLine1, addressLine2, city, countryId, countyId, stateId, zip);
//                if (response.Data == null)
//                {
//                    return NotFound();
//                }
//                if (!response.IsSuccessful)
//                {
//                    return BadRequest(response.ErrorMessages.ToString());
//                }
//
//                var dynamicResponse = _dynamicTransmogrifier.ToDynamicResponse(response, urlHelper, fields);
//
//                return Ok(dynamicResponse);
//            }
//            catch (Exception ex)
//            {
//                _logger.Error(ex);
//                return InternalServerError();
//            }
//        }
//    }
//}
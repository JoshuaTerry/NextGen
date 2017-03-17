using System.Runtime.InteropServices;
using DDI.Services;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Services.ServiceInterfaces;

namespace DDI.WebApi.Controllers
{
    //[Authorize]
    public class AddressesController : ControllerBase<Address>
    {
        protected new IAddressService Service => (IAddressService) base.Service;

        public AddressesController()
            : base(new AddressService())
        {
        }

        protected override Expression<Func<Address, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<Address, object>>[]
            {
                a => a.Country,
                a => a.County,
                a => a.Region1,
                a => a.Region2,
                a => a.Region3,
                a => a.Region4,
                a => a.State
            };
        }

        [HttpGet]
        [Route("api/v1/addresses", Name = RouteNames.Address)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitDefault, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Address, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/addresses/{id}", Name = RouteNames.Address + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/addresses", Name = RouteNames.Address + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Address item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/addresses/{id}", Name = RouteNames.Address + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/addresses/{id}", Name = RouteNames.Address + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/addresses/zip")]
        public IHttpActionResult RefineLocation(string addressLine1 = null, string addressLine2 = null, string city = null, Guid? countryId = null, Guid? countyId = null, Guid? stateId = null, string zip = null)
        {
            try
            {
                var fields = "City,Country,County.Id,County.Description,State.Id,State.CountryId,State.Description,PostalCode,Region1Id,Region2Id,Region3Id,Region4Id,Region1,Region2,Region3,Region4";
                var response = Service.RefineAddress(addressLine1, addressLine2, city, countryId, countyId, stateId, zip);

                return FinalizeResponse(response, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}

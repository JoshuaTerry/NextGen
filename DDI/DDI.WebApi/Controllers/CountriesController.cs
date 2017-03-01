using DDI.Services;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    public class CountriesController : ControllerBase<Country>
    {
        public CountriesController(ServiceBase<Country> service)
            : base(service)
        {
        }
        public CountriesController()
            : base(new CountryService())
        {
        }

        [HttpGet]
        [Route("api/v1/countries", Name = RouteNames.Country)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        { 
            return base.GetAll(RouteNames.Country, limit, offset, orderBy, fields);
        }

        [HttpPost]
        [Route("api/v1/countries", Name = RouteNames.Country + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Country item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/countries/{id}", Name = RouteNames.Country + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Web.Http;
using DDI.Shared.Models.Common;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq; 

namespace DDI.WebApi.Controllers
{
    public class CountriesController : ControllerBase<Country>
    {
        [HttpGet]
        [Route("api/v1/countries", Name = RouteNames.Country)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = "Description", string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.Country, limit, offset, orderBy, fields);
        }

        [HttpPost]
        [Route("api/v1/countries", Name = RouteNames.Country + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Country item)
        {
            return base.Post(GetUrlHelper(), item);
        }

        [HttpPatch]
        [Route("api/v1/countries/{id}", Name = RouteNames.Country + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(GetUrlHelper(), id, changes);
        }
    }
}

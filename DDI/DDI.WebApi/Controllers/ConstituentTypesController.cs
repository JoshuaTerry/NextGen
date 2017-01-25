using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class ConstituentTypesController : ControllerBase<ConstituentType>
    {
        [HttpGet]
        [Route("api/v1/constituenttypes", Name = RouteNames.ConstituentType)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = "DisplayName", string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.ConstituentType, limit, offset, orderBy, fields);
        }

        [HttpPost]
        [Route("api/v1/constituenttypes", Name = RouteNames.ConstituentType + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ConstituentType item)
        {
            return base.Post(GetUrlHelper(), item);
        }

        [HttpPatch]
        [Route("api/v1/constituenttypes/{id}", Name = RouteNames.ConstituentType + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(GetUrlHelper(), id, changes);
        }
    }
}
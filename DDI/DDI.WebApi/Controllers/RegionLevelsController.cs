using System;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class RegionLevelsController : ControllerBase<RegionLevel>
    {

        [HttpGet]
        [Route("api/v1/regionlevels", Name = RouteNames.RegionLevel)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitDefault, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.RegionLevel, string fields = null)
        {
            return base.GetAll(RouteNames.RegionLevel, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/regionlevels/{id}", Name = RouteNames.RegionLevel + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/regionlevels", Name = RouteNames.RegionLevel + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] RegionLevel item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/regionlevels/{id}", Name = RouteNames.RegionLevel + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/regionlevels", Name = RouteNames.RegionLevel + RouteVerbs.Delete)]
        public new IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

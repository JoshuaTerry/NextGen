using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    public class RegionLevelsController : GenericController<RegionLevel>
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
        [Route("api/v1/regionlevels/{id}", Name = RouteNames.RegionLevel + RouteVerbs.Delete)]
        public new IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

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
        ServiceBase<RegionLevel> _service;

        public RegionLevelsController() : this(new ServiceBase<RegionLevel>()) { }
        internal RegionLevelsController(ServiceBase<RegionLevel> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/regionlevels", Name = RouteNames.RegionLevel)]
        public IHttpActionResult GetAll(int? limit = 25, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            orderBy = "ItemLevel";
            var result = _service.GetAll();

            return base.GetAll(GetUrlHelper(), RouteNames.RegionLevel, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/regionlevels/{id}", Name = RouteNames.RegionLevel + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(GetUrlHelper(), id, fields);
        }

        [HttpPost]
        [Route("api/v1/regionlevels", Name = RouteNames.RegionLevel + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] RegionLevel item)
        {
            return base.Post(GetUrlHelper(), item);
        }

        [HttpPatch]
        [Route("api/v1/regionlevels/{id}", Name = RouteNames.RegionLevel + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(GetUrlHelper(), id, changes);
        }

        [HttpDelete]
        [Route("api/v1/regionlevels", Name = RouteNames.RegionLevel + RouteVerbs.Delete)]
        public IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

using System;
using System.Web.Http;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class RegionLevelsController : GenericController<RegionLevel>
    {
        public RegionLevelsController(IService<RegionLevel> service) : base(service) { }

        protected override string FieldsForList => FieldsForAll;
        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.Regions);

        [HttpGet]
        [Route("api/v1/regionlevels")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitDefault, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.RegionLevel, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/regionlevels/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/regionlevels")]
        public IHttpActionResult Post([FromBody] RegionLevel item)
        {
            return base.Post(item);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/regionlevels/{id}")]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/regionlevels/{id}")]
        public new IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

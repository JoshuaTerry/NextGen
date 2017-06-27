using DDI.Shared;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
namespace DDI.WebApi.Controllers.CP
{
    [Authorize]
    public class EFTInfoController : GenericController<EFTInfo>
    {
        public EFTInfoController(IService<EFTInfo> service) : base(service) { }

        [HttpGet]
        [Route("api/v1/eftinfo", Name = RouteNames.EFTInfo)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.EFTInfo, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/eftinfo/{id}", Name = RouteNames.EFTInfo + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/eftinfo", Name = RouteNames.EFTInfo + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] EFTInfo entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/eftinfo/{id}", Name = RouteNames.EFTInfo + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/eftinfo/{id}", Name = RouteNames.EFTInfo + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
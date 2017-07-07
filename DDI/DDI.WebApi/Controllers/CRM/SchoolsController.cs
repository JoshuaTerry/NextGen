using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class SchoolsController : GenericController<School>
    {
        public SchoolsController(IService<School> service) : base(service) { }

        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/schools")]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/schools/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/schools")]
        public IHttpActionResult Post([FromBody] School entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/schools/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);

        }
        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/schools/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

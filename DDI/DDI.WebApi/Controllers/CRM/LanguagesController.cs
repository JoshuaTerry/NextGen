using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class LanguagesController : GenericController<Language>
    {
        public LanguagesController(IService<Language> service) : base(service) { }

        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/languages", Name = RouteNames.Language)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Language, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/languages/{id}", Name = RouteNames.Language + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/languages", Name = RouteNames.Language + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Language entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/languages/{id}", Name = RouteNames.Language + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/languages/{id}", Name = RouteNames.Language + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
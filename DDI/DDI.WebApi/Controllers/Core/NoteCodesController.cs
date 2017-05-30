using System;
using System.Web.Http;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class NoteCodesController : GenericController<NoteCode>
    {
        public NoteCodesController(IService<NoteCode> service) : base(service) { }

        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/notecodes", Name = RouteNames.NoteCode)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.NoteCode, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/notecodes/{id}", Name = RouteNames.NoteCode + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/notecodes", Name = RouteNames.NoteCode + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] NoteCode entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/notecodes/{id}", Name = RouteNames.NoteCode + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/notecodes/{id}", Name = RouteNames.NoteCode + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

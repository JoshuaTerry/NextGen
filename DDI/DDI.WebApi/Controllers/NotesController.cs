using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class NotesController : ControllerBase<Note>
    {
        [HttpGet]
        [Route("api/v1/notes", Name = RouteNames.Note)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Note, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/notes/{id}", Name = RouteNames.Note + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/notes", Name = RouteNames.Note + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Note entityToSave)
        {

            List<NoteTopic> topics = new List<NoteTopic>();
            // entityToSave.NoteTopics.ForEach(n => base.Service.);

            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/notes/{id}", Name = RouteNames.Note + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/notes/{id}", Name = RouteNames.Note + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

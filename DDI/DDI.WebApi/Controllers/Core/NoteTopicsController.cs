using DDI.Services.Search;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.Http;
using DDI.Services;
using DDI.Shared.Helpers;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class NoteTopicsController : GenericController<NoteTopic>
    {
        protected override string FieldsForList => FieldLists.CodeFields;

        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.Notes);

        [HttpGet]
        [Route("api/v1/notetopics", Name = RouteNames.NoteTopic)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.NoteTopic, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/notetopics/{id}", Name = RouteNames.NoteTopic + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/notes/{noteid}/notetopics",Name = RouteNames.Note + RouteNames.NoteTopic)]
        public IHttpActionResult GetByNoteId(Guid noteId, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(n => n.Notes.Any(c => c.Id == noteId), search);
                return FinalizeResponse(response, RouteNames.Note + RouteNames.NoteTopic, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/v1/notetopics", Name = RouteNames.NoteTopic + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] NoteTopic entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/notetopics/{id}", Name = RouteNames.NoteTopic + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/notetopics/{id}", Name = RouteNames.NoteTopic + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpPost]
        [Route("api/v1/notes/{id}/notetopics")]
        public IHttpActionResult AddTopicsToNote(Guid id, [FromBody] JObject topic)
        {
            try
            {
                var noteService = new NoteService();

                var note = noteService.GetById(id).Data;

                if (note == null)
                {
                    return NotFound();
                }

                var response = noteService.AddTopicsToNote(note, topic);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("api/v1/notes/{id}/notetopics/{topicid}")]
        public IHttpActionResult RemoveTopicFromNote(Guid id, Guid topicId)
        {
            try
            {
                var noteService = new NoteService();

                var note = noteService.GetById(id).Data;

                if (note == null)
                {
                    return NotFound();
                }

                var response = noteService.RemoveTopicFromNote(note, topicId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerError(ex);
            }
        }

    }
}

using System.Linq;
using System.Collections.Generic;
using System;
using DDI.Services;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System.Web.Http;
using DDI.Services.ServiceInterfaces;

namespace DDI.WebApi.Controllers
{

    public class NotesController : ControllerBase<Note>
    {
        protected new INoteService Service => (INoteService)base.Service;
        #region Public Constructors

        public NotesController(ServiceBase<Note> service)
            : base(service)
        {
        }

        public NotesController()
            : base(new NoteService())
        {
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpDelete]
        [Route("api/v1/notes/{id}", Name = RouteNames.Note + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpDelete]
        [Route("api/v1/notes/{id}/notetopics/{topicid}")]
        public IHttpActionResult RemoveTopicFromNote(Guid id, Guid topicId)
        {
            try
            {
                var note = Service.GetById(id).Data;
                
                if(note == null)
                {
                    return NotFound();
                }

                var response = Service.RemoveTopicFromNote(note, topicId);

                return Ok(response);
            } catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerError(ex);
            }
        }

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

        [HttpPatch]
        [Route("api/v1/notes/{id}", Name = RouteNames.Note + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpPost]
        [Route("api/v1/notes", Name = RouteNames.Note + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Note entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPost]
        [Route("api/v1/notes/{id}/notetopics")]
        public IHttpActionResult AddTopicsToNote(Guid id, [FromBody] JObject topic)
        {
            try
            {
                var note = Service.GetById(id).Data;

                if(note == null)
                {
                    return NotFound();
                }

                var response = Service.AddTopicsToNote(note, topic);

                return Ok(response);
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerError(ex);
            }
        }

        #endregion Public Methods
    }
}

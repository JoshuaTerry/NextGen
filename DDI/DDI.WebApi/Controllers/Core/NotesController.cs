using DDI.Services;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Services.Search;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class NotesController : GenericController<Note>
    {
        #region Public Properties

        protected new INoteService Service => (INoteService)base.Service;

        #endregion Public Properties

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

        [HttpPost]
        [Route("api/v1/notes/{id}/notetopics")]
        public IHttpActionResult AddTopicsToNote(Guid id, [FromBody] JObject topic)
        {
            try
            {
                var note = Service.GetById(id).Data;

                if (note == null)
                {
                    return NotFound();
                }

                var response = Service.AddTopicsToNote(note, topic);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("api/v1/notes/{id}", Name = RouteNames.Note + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/notes", Name = RouteNames.Note)]
        public IHttpActionResult GetAll(string title = null,
                                        string text = null,
                                        Guid? categoryId = null,
                                        Guid? noteCodeId = null,
                                        Guid? primaryContactId = null,
                                        Guid? contactMethodId = null,
                                        Guid? userResponsibleId = null,
                                        Guid? parentEntityId = null,
                                        string entityType = null,
                                        DateTime? contactDateFrom = null,
                                        DateTime? contactDateTo = null,
                                        DateTime? alertDate = null,
                                        int? limit = SearchParameters.LimitMax, 
                                        int? offset = SearchParameters.OffsetDefault, 
                                        string orderBy = OrderByProperties.DisplayName, 
                                        string fields = null)
        {
            var search = new NoteSearch()
            {
                AlertDate = alertDate,
                CategoryId = categoryId,
                ContactDateFrom = contactDateFrom,
                ContactDateTo = contactDateTo,
                ContactMethodId = contactMethodId,
                EntityType = entityType,
                NoteCodeId = noteCodeId,
                ParentEntityId = parentEntityId,
                PrimaryContactId = primaryContactId,
                UserResponsibleId = userResponsibleId,
                Text = text,
                Title = title,
                Offset = offset,
                Limit = limit,
                OrderBy = orderBy
            };

            return base.GetAll(RouteNames.Note, search, fields);
        }

        [HttpGet]
        [Route("api/v1/notes/{id}", Name = RouteNames.Note + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/{parentid}/notes")]
        public IHttpActionResult GetByEntityId(Guid parentId)
        {
            var result = Service.GetAllWhereExpression(nd => nd.ParentEntityId == parentId);

            if (result == null)
            {
                return NotFound();
            }

            if(!result.IsSuccessful)
            {
                return InternalServerError();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("api/v1/notealert/{parentid}")]
        public IHttpActionResult GetNotesWithinAlertDateRange(Guid parentId)
        {
            var result = Service.GetNotesInAlertDateRange(parentId);

            if (result == null)
            {
                return NotFound();
            }

            if (!result.IsSuccessful)
            {
                return InternalServerError();
            }

            return Ok(result);
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

        [HttpDelete]
        [Route("api/v1/notes/{id}/notetopics/{topicid}")]
        public IHttpActionResult RemoveTopicFromNote(Guid id, Guid topicId)
        {
            try
            {
                var note = Service.GetById(id).Data;

                if (note == null)
                {
                    return NotFound();
                }

                var response = Service.RemoveTopicFromNote(note, topicId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerError(ex);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override Expression<Func<Note, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<Note, object>>[]
            {
                n => n.NoteTopics
            };
        }

        #endregion Protected Methods
    }
}

using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class NotesController : GenericController<Note>
    {
        #region Public Properties

        protected new INoteService Service => (INoteService)base.Service;

        protected override string FieldsForList => $"{nameof(Note.Id)},{nameof(Note.DisplayName)}";

        protected override string FieldsForAll => FieldListBuilder
            .IncludeAll()
            .Exclude(p => p.Category)
            .Exclude(p => p.ContactMethod)
            .Exclude(p => p.NoteCode)
            .Include(p => p.NoteTopics.First().Id)
            .Include(p => p.NoteTopics.First().DisplayName);

        #endregion Public Properties

        #region Public Constructors

        public NotesController(INoteService service)
            : base(service)
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
        [Route("api/v1/entity/{parentid}/notes", Name = RouteNames.Entity + RouteNames.Note)]
        public IHttpActionResult GetByEntityId(Guid parentId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var result = Service.GetAllWhereExpression(nd => nd.ParentEntityId == parentId, search, fields);

                return FinalizeResponse(result, RouteNames.Entity + RouteNames.Note, search, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/entity/{parentid}/notes/alert", Name = RouteNames.Entity + RouteNames.Note + RouteNames.Alert)]
        public IHttpActionResult GetNotesWithinAlertDateRange(Guid parentId)
        {
            try
            {
                var search = PageableSearch.Max;
                var result = Service.GetNotesInAlertDateRange(parentId);

                return FinalizeResponse(result, RouteNames.Entity + RouteNames.Note + RouteNames.Alert, search,
                    $"{nameof(Note.Id)},{nameof(Note.AlertStartDate)},{nameof(Note.AlertEndDate)},{nameof(Note.Title)}");
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

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

using DDI.Services;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Services.Search;
using DDI.Shared.Helpers;
using System.Linq;
using DDI.Shared;

namespace DDI.WebApi.Controllers.General
{
    //[Authorize]
    public class AttachmentController : GenericController<Attachment>
    {
        #region Public Properties

        protected new IAttachmentService Service => (IAttachmentService)base.Service;

        #endregion Public Properties

        #region Public Constructors

        public AttachmentController(IAttachmentService service) : base(service) { }

        #endregion Public Constructors

        #region Public Methods


        [HttpDelete]
        [Route("api/v1/attachments/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            try
            {
                var attachmentDelete = Service.Delete(id);

                return Ok(attachmentDelete);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/attachments")]
        public override IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax,
                                        int? offset = SearchParameters.OffsetDefault,
                                        string orderBy = OrderByProperties.DisplayName,
                                        string fields = null)
        {
            var search = new PageableSearch()
            {
                Offset = offset,
                Limit = limit,
                OrderBy = orderBy
            };

            return base.GetAll(search, fields);
        }

        [HttpGet]
        [Route("api/v1/attachments/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/attachments/entity/{entityId}")]
        public IHttpActionResult GetByEntityId(Guid entityId)
        {
            try
            {
                var search = PageableSearch.Max;
                var result = Service.GetAttachmentsForEntityId(entityId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/attachments/notes/{noteId}")]
        public IHttpActionResult GetAttachmensByNoteId( Guid noteId)
        {
            try
            {
                var search = PageableSearch.Max;
                var result = Service.GetAttachmentsForNoteId(noteId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }

        [HttpPatch]
        [Route("api/v1/attachments/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpPost]
        [Route("api/v1/attachments")]
        public override IHttpActionResult Post([FromBody] Attachment entityToSave)
        {
            return base.Post(entityToSave);
        }

       
        #endregion Protected Methods
    }
}

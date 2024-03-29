﻿using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class NoteContactMethodsController : GenericController<NoteContactMethod>
    {
        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/notecontactcodes", Name = RouteNames.NoteContactCode)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.NoteContactCode, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/notecontactcodes/{id}", Name = RouteNames.NoteContactCode + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/notecontactcodes", Name = RouteNames.NoteContactCode + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] NoteContactMethod entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/notecontactcodes/{id}", Name = RouteNames.NoteContactCode + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/notecontactcodes/{id}", Name = RouteNames.NoteContactCode + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

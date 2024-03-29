﻿using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class ContactCategoryController : GenericController<ContactCategory>
    {
        protected override string FieldsForList => FieldLists.CodeFields + ",SectionTitle";

        [HttpGet]
        [Route("api/v1/contactcategory", Name = RouteNames.ContactCategory)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ContactInfo, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/contactcategory/{id}", Name = RouteNames.ContactCategory + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/contactcategory", Name = RouteNames.ContactCategory + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ContactCategory entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/contactcategory/{id}", Name = RouteNames.ContactCategory + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/contactcategory/{id}", Name = RouteNames.ContactCategory + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }

}


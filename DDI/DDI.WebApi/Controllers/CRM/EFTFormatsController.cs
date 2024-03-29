﻿using DDI.Shared.Models.Client.CP;
using DDI.Shared.Statics;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class EFTFormatsController : GenericController<EFTFormat>
    {
        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/eftformats")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll("", limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/eftformats/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }
    }
}

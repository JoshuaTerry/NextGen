﻿using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class ConstituentStatusesController : GenericController<ConstituentStatus>
    {
        public ConstituentStatusesController(IService<ConstituentStatus> service) : base(service) { }

        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/constituentstatuses")]
        public override IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/constituentstatuses")]
        public override IHttpActionResult Post([FromBody] ConstituentStatus item)
        {
            return base.Post(item);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/constituentstatuses/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }
    }
}
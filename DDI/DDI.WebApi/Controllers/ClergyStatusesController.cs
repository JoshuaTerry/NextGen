﻿using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class ClergyStatusesController : ControllerBase<ClergyStatus>
    {
        [HttpGet]
        [Route("api/v1/clergystatuses", Name = RouteNames.ClergyStatus)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderby = "DisplayName", string fields = null)
        {
            return base.GetAll(RouteNames.ClergyStatus, limit, offset, orderby, fields);
        }

        [HttpPost]
        [Route("api/v1/clergystatuses", Name = RouteNames.ClergyStatus + RouteVerbs.Post)]
        public override IHttpActionResult Post([FromBody] ClergyStatus item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/clergystatuses/{id}", Name = RouteNames.ClergyStatus + RouteVerbs.Patch)]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }
    }
}
﻿using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class ConstituentStatusesController : ControllerBase<ConstituentStatus>
    {
        [HttpGet]
        [Route("api/v1/constituentstatuses", Name = RouteNames.ConstituentStatus)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = "DisplayName", string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.ConstituentStatus, limit, offset, orderBy, fields);
        }

        [HttpPost]
        [Route("api/v1/constituentstatuses", Name = RouteNames.ConstituentStatus + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ConstituentStatus item)
        {
            return base.Post(GetUrlHelper(), item);
        }

        [HttpPatch]
        [Route("api/v1/constituentstatuses/{id}", Name = RouteNames.ConstituentStatus + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(GetUrlHelper(), id, changes);
        }
    }
}
﻿using System;
using System.Data;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class ClergyTypesController : ControllerBase<ClergyType>
    {
        [HttpGet]
        [Route("api/v1/clergytypes", Name = RouteNames.ClergyType)]
        public IHttpActionResult GetAll(int? limit = 25, int? offset = 0, string orderby = "DisplayName", string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.ClergyType, limit, offset, orderby, fields);
        }

        [HttpPost]
        [Route("api/v1/clergytypes", Name = RouteNames.ClergyType + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ClergyType item)
        {
            return base.Post(GetUrlHelper(), item);
        }

        [HttpPatch]
        [Route("api/v1/clergytypes/{id}", Name = RouteNames.ClergyType + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(GetUrlHelper(), id, changes);
        }
    }
}
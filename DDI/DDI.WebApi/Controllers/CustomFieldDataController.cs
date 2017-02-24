﻿using System;
using System.Web.Http;
using DDI.Shared.Models.Client.Core;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class CustomFieldDataController : ControllerBase<CustomFieldData>
    {
        [HttpGet]
        [Route("api/v1/customfielddata")]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.CustomFieldData, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/customfielddata/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/customfielddata")]
        public IHttpActionResult Post([FromBody] CustomFieldData item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/customfielddata/{id}")]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/customfielddata/{id}")]
        public IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

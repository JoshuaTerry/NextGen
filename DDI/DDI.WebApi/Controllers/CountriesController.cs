﻿using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Web.Http;
using DDI.Shared.Models.Common;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq; 

namespace DDI.WebApi.Controllers
{
    public class CountriesController : ControllerBase<Country>
    {
        [HttpGet]
        [Route("api/v1/countries", Name = RouteNames.Country)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderby = "Description", string fields = null)
        {
            return base.GetAll(RouteNames.Country, limit, offset, orderby, fields);
        }

        [HttpPost]
        [Route("api/v1/countries", Name = RouteNames.Country + RouteVerbs.Post)]
        public override IHttpActionResult Post([FromBody] Country item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/countries/{id}", Name = RouteNames.Country + RouteVerbs.Patch)]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }
    }
}

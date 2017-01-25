﻿using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using DDI.Services;
using DDI.Services.Extensions;
using Newtonsoft.Json.Linq;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;

namespace DDI.WebApi.Controllers
{
    //[Authorize]
    public class ConstituentAddressesController : ControllerBase<ConstituentAddress>
    {
        [HttpGet]
        [Route("api/v1/constituentAddresses", Name = RouteNames.ConstituentAddress)]
        public IHttpActionResult GetAll(int? limit = 25, int? offset = 0, string orderBy = "DisplayName", string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.ConstituentAddress, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/constituentaddresses/{id}", Name = RouteNames.ConstituentAddress + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(GetUrlHelper(), id, fields);
        }

        [HttpPost]
        [Route("api/v1/constituentaddresses", Name = RouteNames.ConstituentAddress + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ConstituentAddress item)
        {
            return base.Post(GetUrlHelper(), item);
        }

        [HttpPatch]
        [Route("api/v1/constituentaddresses/{id}", Name = RouteNames.ConstituentAddress + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(GetUrlHelper(), id, changes);
        }

        [HttpDelete]
        [Route("api/v1/constituentaddresses/{id}", Name = RouteNames.ConstituentAddress + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

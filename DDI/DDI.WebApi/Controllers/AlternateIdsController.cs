using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class AlternateIdsController : ControllerBase<AlternateId>
    {
        [HttpGet]
        [Route("api/v1/alternateid", Name = RouteNames.AlternateId)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.AlternateId, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/alternateid/{id}", Name = RouteNames.AlternateId + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(GetUrlHelper(), id, fields);
        }

        [HttpPost]
        [Route("api/v1/alternateid", Name = RouteNames.AlternateId + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] AlternateId entityToSave)
        {
            return base.Post(GetUrlHelper(), entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/alternateid/{id}", Name = RouteNames.AlternateId + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(GetUrlHelper(), id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/alternateid/{id}", Name = RouteNames.AlternateId + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
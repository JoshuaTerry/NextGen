using System;
using System.Web.Http;
using DDI.Shared.Models.Client.Core;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class CustomFieldsController : ControllerBase<CustomField>
    {
        [HttpGet]
        [Route("api/v1/customfields", Name = RouteNames.CustomField)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderby = "DisplayName", string fields = null)
        {
            return base.GetAll(RouteNames.CustomField, limit, offset, orderby, fields);
        }

        [HttpPost]
        [Route("api/v1/customfields", Name = RouteNames.CustomField + RouteVerbs.Post)]
        public override IHttpActionResult Post([FromBody] CustomField item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/customfields/{id}", Name = RouteNames.CustomField + RouteVerbs.Patch)]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }
    }
}
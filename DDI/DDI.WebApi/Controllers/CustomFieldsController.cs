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
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.CustomField, limit, offset, orderBy, fields);
        }

        [HttpPost]
        [Route("api/v1/customfields", Name = RouteNames.CustomField + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] CustomField item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/customfields/{id}", Name = RouteNames.CustomField + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }
    }
}
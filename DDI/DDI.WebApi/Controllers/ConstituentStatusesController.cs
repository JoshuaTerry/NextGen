using System;
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
        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/constituentstatuses", Name = RouteNames.ConstituentStatus)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ConstituentStatus, limit, offset, orderBy, fields);
        }

        [HttpPost]
        [Route("api/v1/constituentstatuses", Name = RouteNames.ConstituentStatus + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ConstituentStatus item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/constituentstatuses/{id}", Name = RouteNames.ConstituentStatus + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }
    }
}
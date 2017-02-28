using System;
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
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ClergyStatus, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/clergystatuses/{id}", Name = RouteNames.ClergyStatus + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/clergystatuses", Name = RouteNames.ClergyStatus + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ClergyStatus item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/clergystatuses/{id}", Name = RouteNames.ClergyStatus + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }
    }
}
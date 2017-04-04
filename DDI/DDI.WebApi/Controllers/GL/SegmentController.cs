using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.GL
{
    public class SegmentController : GeneralLedgerController<Segment>
    {
        [HttpGet]
        [Route("api/v1/businessunit/{businessUnitId}/segments", Name = RouteNames.Segment)]
        public IHttpActionResult GetAll(Guid businessUnitId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Segment, businessUnitId, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/segments/{id}", Name = RouteNames.Segment + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/segments", Name = RouteNames.Segment + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Segment item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/segments/{id}", Name = RouteNames.Segment + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/segments/{id}", Name = RouteNames.Segment + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
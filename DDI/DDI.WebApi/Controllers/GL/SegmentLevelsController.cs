using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using DDI.Services.Search;

namespace DDI.WebApi.Controllers.GL
{
    public class SegmentLevelsController : GeneralLedgerController<SegmentLevel>
    {

        [HttpGet]
        [Route("api/v1/segmentlevels/{id}", Name = RouteNames.SegmentLevel + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/segmentlevels/ledger/{id}", Name = RouteNames.SegmentLevel + RouteNames.Ledger + RouteVerbs.Get)]
        public IHttpActionResult GetByLedgerId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {

            try
            {
                var search = new PageableSearch(SearchParameters.OffsetDefault, SearchParameters.LimitDefault, "Level");
                var response = Service.GetAllWhereExpression(a => a.LedgerId == id, search);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("api/v1/segmentlevels", Name = RouteNames.SegmentLevel + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] SegmentLevel item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/segmentlevels/{id}", Name = RouteNames.SegmentLevel + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/segmentlevels/{id}", Name = RouteNames.SegmentLevel + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
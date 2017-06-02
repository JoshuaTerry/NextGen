using System;
using System.Web.Http;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class SegmentLevelsController : GenericController<SegmentLevel>
    {
        public SegmentLevelsController(IService<SegmentLevel> service) : base(service) { }


        private const string ROUTENAME_GETBYLEDGERID = RouteNames.Ledger + RouteNames.SegmentLevel + RouteVerbs.Get;
        private const string ROUTENAME_GETBYUNITID = RouteNames.BusinessUnit + RouteNames.SegmentLevel + RouteVerbs.Get;

        protected override string FieldsForAll => FieldListBuilder
            .IncludeAll()
            .Exclude(p => p.Ledger)
            .Exclude(p => p.Segments);

        protected override string FieldsForList => FieldsForAll;

        [HttpGet]
        [Route("api/v1/segmentlevels/{id}", Name = RouteNames.SegmentLevel + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/segmentlevels/ledger/{id}", Name = RouteNames.SegmentLevel + RouteNames.Ledger + RouteVerbs.Get)]
        [Route("api/v1/ledgers/{id}/segmentlevels", Name = ROUTENAME_GETBYLEDGERID)]
        public IHttpActionResult GetByLedgerId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = nameof(SegmentLevel.Level))
        {

            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.LedgerId == id, search);
                
                return FinalizeResponse(response, ROUTENAME_GETBYLEDGERID, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }


        [HttpGet]
        [Route("api/v1/segmentlevels/businessunit/{id}", Name = RouteNames.SegmentLevel + RouteNames.BusinessUnit + RouteVerbs.Get)]
        [Route("api/v1/businessunits/{id}/segmentlevels", Name = ROUTENAME_GETBYUNITID)]
        public IHttpActionResult GetByUnitId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = nameof(SegmentLevel.Level))
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.Ledger.BusinessUnitId == id, search);

                return FinalizeResponse(response, ROUTENAME_GETBYUNITID, search, ConvertFieldList(fields, FieldsForList));
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
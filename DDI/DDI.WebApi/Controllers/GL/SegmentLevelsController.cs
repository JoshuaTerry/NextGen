using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class SegmentLevelsController : GenericController<SegmentLevel>
    {
        public SegmentLevelsController(IService<SegmentLevel> service) : base(service) { }

        protected override string FieldsForAll => FieldListBuilder
            .IncludeAll()
            .Exclude(p => p.Ledger)
            .Exclude(p => p.Segments);

        protected override string FieldsForList => FieldsForAll;

        [HttpGet]
        [Route("api/v1/segmentlevels/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/segmentlevels/ledger/{id}")]
        [Route("api/v1/ledgers/{id}/segmentlevels")]
        public IHttpActionResult GetByLedgerId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = nameof(SegmentLevel.Level))
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var response = Service.GetAllWhereExpression(a => a.LedgerId == id, search, fields);

                return FinalizeResponse(response, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }


        [HttpGet]
        [Route("api/v1/segmentlevels/businessunit/{id}")]
        [Route("api/v1/businessunits/{id}/segmentlevels")]
        public IHttpActionResult GetByUnitId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = nameof(SegmentLevel.Level))
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var response = Service.GetAllWhereExpression(a => a.Ledger.BusinessUnitId == id, search, fields);

                return FinalizeResponse(response, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("api/v1/segmentlevels")]
        public override IHttpActionResult Post([FromBody] SegmentLevel item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/segmentlevels/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/segmentlevels/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
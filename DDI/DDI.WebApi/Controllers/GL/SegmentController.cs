using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class SegmentController : GenericController<Segment>
    {
        protected override string FieldsForList => $"{nameof(Segment.Id)},{nameof(Segment.Code)},{nameof(Segment.Name)},{nameof(Segment.DisplayName)}";

        protected override string FieldsForAll => FieldListBuilder
            .Exclude(p => p.AccountSegments)
            .Exclude(p => p.ChildSegments)
            .Exclude(p => p.ParentSegment)
            .Exclude(p => p.FiscalYear)
            .Exclude(p => p.SegmentLevel.Ledger)
            .Exclude(p => p.SegmentLevel.Segments);

        protected override Expression<Func<Segment, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<Segment, object>>[]
            {
                p => p.SegmentLevel
            };
        }

        protected new ISegmentService Service => (ISegmentService)base.Service;

        public SegmentController(ISegmentService service)
            : base(service)
        {
        }


        [HttpGet]
        [Route("api/v1/fiscalyears/{yearid}/segments/level/{level}")]
        public IHttpActionResult GetAll(Guid yearId, string level, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            var search = new PageableSearch(offset, limit, orderBy);

            try
            {
                var result = Service.GetSegmentSearch(yearId, null, level, search);
                return FinalizeResponse(result, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }


        [HttpGet]
        [Route("api/v1/fiscalyears/{yearid}/segments/{parentId}/level/{level}")]
        public IHttpActionResult GetAllForParent(Guid yearId, Guid? parentId, string level, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            var search = new PageableSearch(offset, limit, orderBy);

            try
            {
                var result = Service.GetSegmentSearch(yearId, parentId, level, search);
                return FinalizeResponse(result, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/segments/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = "all")
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/segments")]
        public override IHttpActionResult Post([FromBody] Segment item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/segments/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/segments/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
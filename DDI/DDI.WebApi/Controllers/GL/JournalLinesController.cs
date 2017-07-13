using DDI.Services.GL;
using DDI.Services.Search;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Shared;

namespace DDI.WebApi.Controllers.GL
{
    public class JournalLinesController : GenericController<JournalLine>
    {
        public JournalLinesController(IService<JournalLine> serviceBase) : base(serviceBase)
        {
        }

        protected override Expression<Func<JournalLine, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<JournalLine, object>>[]
            {
                c => c.LedgerAccount,
                c => c.SourceBusinessUnit,
                c => c.SourceFund,
                c => c.SourceFund.FundSegment,
                c => c.Journal
            };
        }

        protected override Expression<Func<JournalLine, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<JournalLine, object>>[]
            {
                c => c.LedgerAccount,
                c => c.SourceBusinessUnit,
                c => c.SourceFund,
                c => c.Journal
            };
        }

        [HttpGet]
        [Route("api/v1/journalline")]
        public new IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/journalline/journal/{journalId}")]
        public IHttpActionResult GetAllByJournalId(Guid journalId, int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            try
            {
                orderBy = "LineNumber";
                fields = ConvertFieldList(fields, FieldsForList);
                var search = new PageableSearch(offset, limit, orderBy);
                var result = Service.GetAllWhereExpression(j => j.JournalId == journalId, search, fields);

                return FinalizeResponse(result, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/journalline/{id}")]
        public new IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize]
        [HttpPost]
        [Route("api/v1/journalline")]
        public new IHttpActionResult Post([FromBody] JournalLine entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize]
        [HttpPatch]
        [Route("api/v1/journalline/{id}")]
        public new IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize]
        [HttpDelete]
        [Route("api/v1/journalline/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

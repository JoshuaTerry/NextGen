using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class FiscalPeriodController : GenericController<FiscalPeriod>
    {
        public FiscalPeriodController(IService<FiscalPeriod> service) : base(service) { }
        
        protected override string FieldsForList => $"{nameof(FiscalPeriod.Id)},{nameof(FiscalPeriod.PeriodNumber)},{nameof(FiscalPeriod.StartDate)},{nameof(FiscalPeriod.EndDate)},{nameof(FiscalPeriod.Status)}";

        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.FiscalYear);

        [HttpGet]
        [Route("api/v1/fiscalperiods/fiscalyear/{fiscalYearId}")]
        [Route("api/v1/fiscalyears/{fiscalYearId}/fiscalperiods")]
        public IHttpActionResult GetAllByFiscalYearId(Guid fiscalYearId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = nameof(FiscalPeriod.PeriodNumber), string fields = null)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var result = Service.GetAllWhereExpression(fp => fp.FiscalYearId == fiscalYearId, search, fields);

                return FinalizeResponse(result, search, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/fiscalperiods/glaccount/{accountId}")]
        public IHttpActionResult GetFiscalPeriodsByAccountId(Guid accountId)
        {
            try
            {
                var result = ((IFiscalPeriodService)Service).GetFiscalPeriodsByAccountId(accountId);

                return FinalizeResponse(result, PageableSearch.Max, null);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
        [HttpGet]
        [Route("api/v1/fiscalperiods/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/fiscalperiods")]
        public IHttpActionResult Post([FromBody] FiscalPeriod entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/fiscalperiods/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/fiscalperiods/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

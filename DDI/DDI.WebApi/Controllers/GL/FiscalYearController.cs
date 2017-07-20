using DDI.Services.GL;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class FiscalYearController : GenericController<FiscalYear>
    {
        protected new IFiscalYearService Service => (IFiscalYearService)base.Service;
        public FiscalYearController(IFiscalYearService service) : base(service) { }

        private string _fieldsForAll = null;

        protected override string FieldsForList => $"{nameof(FiscalYear.Id)},{nameof(FiscalYear.Name)},{nameof(FiscalYear.Status)}";

        protected override string FieldsForAll => _fieldsForAll ?? (_fieldsForAll = FieldListBuilder
            .IncludeAll()
            .Include(p => p.Ledger.Name)
            .Include(p => p.Ledger.Code)
            .Exclude(p => p.LedgerAccounts)
            .Exclude(p => p.Segments)
            .Include(p => p.FiscalPeriods.First().Id)
            .Include(p => p.FiscalPeriods.First().PeriodNumber)
            .Include(p => p.FiscalPeriods.First().StartDate)
            .Include(p => p.FiscalPeriods.First().EndDate)
            .Include(p => p.FiscalPeriods.First().Status)
            .Include(p => p.FiscalPeriods.First().IsAdjustmentPeriod)
            );

        protected override Expression<Func<FiscalYear, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<FiscalYear, object>>[]
            {
                p => p.FiscalPeriods,
                p => p.Ledger
            };
        }

        [HttpGet]
        [Route("api/v1/fiscalyears/ledger/{ledgerId}")]
        [Route("api/v1/ledgers/{ledgerId}/fiscalyears")]
        public IHttpActionResult GetAllByLedgerId(Guid ledgerId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = "all")
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var result = Service.GetAllWhereExpression(fy => fy.LedgerId == ledgerId, search, fields);

                return FinalizeResponse(result, search, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/businessunits/{unitId}/fiscalyears/{fiscalYearId}")]
        public IHttpActionResult GetYearForOtherBusinessUnit(Guid unitId, Guid fiscalYearId, string fields = null)
        {
            try
            {
                var result = ((IFiscalYearService)Service).GetYearForOtherBusinessUnit(unitId, fiscalYearId);

                return FinalizeResponse(result, PageableSearch.Default, ConvertFieldList(fields, FieldsForSingle));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
        [HttpGet]
        [Route("api/v1/businessunits/{unitId}/currentfiscalyear")]
        public IHttpActionResult GetCurrentFiscalYear(Guid unitId, string fields = null)
        {
            try
            {
                var result = Service.GetWhereExpression(fy => fy.Ledger.BusinessUnitId == unitId && fy.Id == fy.Ledger.DefaultFiscalYearId);

                return FinalizeResponse(result, ConvertFieldList(fields, FieldsForSingle));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/fiscalyears/businessunit/{unitId}")]
        [Route("api/v1/businessunits/{unitId}/fiscalyears")]
        public IHttpActionResult GetAllByBusinessUnitId(Guid unitId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var result = Service.GetAllWhereExpression(fy => fy.Ledger.BusinessUnitId == unitId, search, fields);

                return FinalizeResponse(result, search, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/fiscalyears/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/fiscalyears")]
        public override IHttpActionResult Post([FromBody] FiscalYear entityToSave)
        {
            return Ok(Service.Post(entityToSave));
        }

        [HttpPost]
        [Route("api/v1/fiscalyears/{sourceFiscalYearId}/copy")]
        public IHttpActionResult Copy(Guid sourceFiscalYearId, [FromBody] FiscalYearTemplate newFiscalYear)
        {
            try
            {
                return FinalizeResponse(((FiscalYearService)Service).CopyFiscalYear(sourceFiscalYearId, newFiscalYear), FieldsForSingle);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPatch]
        [Route("api/v1/fiscalyears/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/fiscalyears/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

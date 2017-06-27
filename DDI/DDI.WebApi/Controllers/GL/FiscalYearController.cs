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
        public FiscalYearController(IService<FiscalYear> service) : base(service) { }

        private const string ROUTENAME_GETALLBYLEDGER = RouteNames.Ledger + RouteNames.FiscalYear + RouteVerbs.Get;
        private const string ROUTENAME_GETALLBYUNIT = RouteNames.BusinessUnit + RouteNames.FiscalYear + RouteVerbs.Get;
        private const string ROUTENAME_GETYEARFOROTHERBUSINESSUNIT = RouteNames.BusinessUnit + RouteNames.FiscalYear + "Id" + RouteVerbs.Get;

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
        [Route("api/v1/fiscalyears/ledger/{ledgerId}", Name = RouteNames.FiscalYear + RouteNames.Ledger + RouteVerbs.Get)]
        [Route("api/v1/ledgers/{ledgerId}/fiscalyears", Name = ROUTENAME_GETALLBYLEDGER)]
        public IHttpActionResult GetAllByLedgerId(Guid ledgerId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = "all")
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var result = Service.GetAllWhereExpression(fy => fy.LedgerId == ledgerId, search, fields);

                return FinalizeResponse(result, ROUTENAME_GETALLBYLEDGER, search, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/businessunits/{unitId}/fiscalyears/{fiscalYearId}", Name = ROUTENAME_GETYEARFOROTHERBUSINESSUNIT)]
        public IHttpActionResult GetYearForOtherBusinessUnit(Guid unitId, Guid fiscalYearId, string fields = null)
        {
            try
            {
                var result = ((IFiscalYearService)Service).GetYearForOtherBusinessUnit(unitId, fiscalYearId);

                return FinalizeResponse(result, ROUTENAME_GETYEARFOROTHERBUSINESSUNIT, PageableSearch.Default, ConvertFieldList(fields, FieldsForSingle));
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

                return FinalizeResponse(result, ConvertFieldList(fields, FieldsForSingle), null);
            }
            catch(Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/fiscalyears/businessunit/{unitId}", Name = RouteNames.FiscalYear + RouteNames.BusinessUnit + RouteVerbs.Get)]
        [Route("api/v1/businessunits/{unitId}/fiscalyears", Name = ROUTENAME_GETALLBYUNIT)]
        public IHttpActionResult GetAllByBusinessUnitId(Guid unitId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var result = Service.GetAllWhereExpression(fy => fy.Ledger.BusinessUnitId == unitId, search, fields);

                return FinalizeResponse(result, ROUTENAME_GETALLBYUNIT, search, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/fiscalyears/{id}", Name = RouteNames.FiscalYear + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/fiscalyears", Name = RouteNames.FiscalYear + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] FiscalYear entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/fiscalyears/{id}", Name = RouteNames.FiscalYear + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/fiscalyears/{id}", Name = RouteNames.FiscalYear + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

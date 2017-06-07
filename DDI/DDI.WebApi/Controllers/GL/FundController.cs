using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDI.Services.Search;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq.Expressions;
using DDI.Services.GL;
using DDI.Services.ServiceInterfaces;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class FundController : GenericController<Fund>
    {

        private const string ROUTENAME_FUND = RouteNames.Ledger + RouteNames.Fund + RouteVerbs.Get;

        public FundController(FundService service) : base(service) { }

        protected override Expression<Func<Fund, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<Fund, object>>[]
            {
                c => c.FundSegment,
                c => c.ClosingExpenseLedgerAccount,
                c => c.FundBalanceLedgerAccount,
                c => c.ClosingRevenueLedgerAccount,
                c => c.FiscalYear
            };
        }

        protected override Expression<Func<Fund, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<Fund, object>>[]
            {
                c => c.FundSegment,
                c => c.ClosingExpenseLedgerAccount,
                c => c.FundBalanceLedgerAccount,
                c => c.ClosingRevenueLedgerAccount,
                c => c.FiscalYear
            };
        }

        protected override string FieldsForList => FieldListBuilder
            .Include(p => p.FiscalYearId)
            .Include(p => p.FiscalYear.Name)
            .Include(p => p.CreatedBy)
            .Include(p => p.CreatedOn)
            .Include(p => p.DisplayName)
            .Include(p => p.FundSegment.Code)
            .Include(p => p.FundSegment.Name)
            .Include(p => p.FundSegmentId)
            .Include(p => p.FundBalanceLedgerAccount.AccountNumber)
            .Include(p => p.FundBalanceLedgerAccount.Name)
            .Include(p => p.FundBalanceLedgerAccountId)
            .Include(p => p.ClosingRevenueLedgerAccount.AccountNumber)
            .Include(p => p.ClosingRevenueLedgerAccount.Name)
            .Include(p => p.ClosingRevenueLedgerAccountId)
            .Include(p => p.ClosingExpenseLedgerAccount.AccountNumber)
            .Include(p => p.ClosingExpenseLedgerAccount.Name)
            .Include(p => p.ClosingExpenseLedgerAccountId)
            .Include(p => p.LastModifiedBy)
            .Include(p => p.LastModifiedOn)
            .Include(p => p.Id)
            .Include(p => p.FundBalanceAccountId)
            .Include(p => p.ClosingRevenueAccountId)
            .Include(p => p.ClosingExpenseAccountId);

        protected override string FieldsForAll => FieldsForList;

        [HttpGet]
        [Route("api/v1/fund", Name = RouteNames.Fund)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Fund, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/fund/{id}", Name = RouteNames.Fund + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/fund/{fiscalyearid}/fiscalyear")]
        public IHttpActionResult GetByFiscalYearId(Guid fiscalyearid, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.Order)
        {
            try
            {
                orderBy= orderBy == "Order"  ? "DisplayName": orderBy;
                var search = new PageableSearch(offset, limit, orderBy);
                var result = Service.GetAllWhereExpression(f => f.FiscalYearId == fiscalyearid , search);
                
                return FinalizeResponse(result, "", search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/v1/fund", Name = RouteNames.Fund + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Fund entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize]
        [HttpPatch]
        [Route("api/v1/fund/{id}", Name = RouteNames.Fund + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize]
        [HttpDelete]
        [Route("api/v1/fund/{id}", Name = RouteNames.Fund + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

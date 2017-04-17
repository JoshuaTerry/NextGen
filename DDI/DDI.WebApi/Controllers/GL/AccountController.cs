using DDI.Services;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Services.Search;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class AccountController : GeneralLedgerController<Account>
    {
        private ServiceBase<GLAccountSelection> _glAccount = new ServiceBase<GLAccountSelection>();
        private ServiceBase<Ledger> _ledger = new ServiceBase<Ledger>();

        [HttpGet]
        [Route("api/v1/businessunit/{businessUnitId}/accounts", Name = RouteNames.Account)]
        public IHttpActionResult GetAll(Guid businessUnitId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Account, businessUnitId, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/accounts/fiscalyear/{id}")]
        public IHttpActionResult GetAllLedgerAccounts(Guid Id, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            var search = new PageableSearch(null, 25000, null);
           
            var accounts = _glAccount.GetAllWhereExpression(l => l.FiscalYearId == Id , search);

            if (accounts == null)
            {
                NotFound();
            }

            return Ok(accounts);
        }

        [HttpGet]
        [Route("api/v1/accounts/lookup/{name}/{ledgerId}/{fiscalYearId}")]
        public IHttpActionResult AccountLookup(string name, Guid ledgerId, Guid fiscalYearId)
        {
            string fields = "Id,AccountNumber,Description";

            var search = new AccountNumberSearch()
            {
                QuickSearch = name,
                Offset = SearchParameters.OffsetDefault,
                Limit = 500,
                Fields = fields,
            };

            return FinalizeResponse(_glAccount.GetAllWhereExpression((a=> a.LedgerId == ledgerId && a.FiscalYearId == fiscalYearId),search),null,search, search.Fields, null);
        }

        [HttpGet]
        [Route("api/v1/accounts/{id}", Name = RouteNames.Account + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/accounts", Name = RouteNames.Account + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Account item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/accounts/{id}", Name = RouteNames.Account + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/accounts/{id}", Name = RouteNames.Account + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
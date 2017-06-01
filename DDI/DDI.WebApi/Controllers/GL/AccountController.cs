using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DDI.Services;
using DDI.Services.GL;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using DevExtreme.AspNet.Data;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class AccountController : GenericController<Account>
    {
        protected new IAccountService Service => (IAccountService)base.Service;

        private const string ROUTENAME_GETALLLEDGERACCOUNTS = RouteNames.Account + RouteNames.FiscalYear + RouteVerbs.Get;

        private ServiceBase<GLAccountSelection> _glAccount;
        private ServiceBase<Ledger> _ledger;
        private ServiceBase<FiscalYear> _fiscalYear;
        private string _fieldsForAll = null;

        protected override string FieldsForList => $"{nameof(Account.Id)},{nameof(Account.AccountNumber)},{nameof(Account.Name)}";
        protected override string FieldsForAll => _fieldsForAll ?? (_fieldsForAll = FieldListBuilder
                                                        .IncludeAll()
                                                        .Exclude(p => p.FiscalYear)
                                                        .Exclude(p => p.NextYearAccounts)
                                                        .Exclude(p => p.PriorYearAccounts)
                                                        .Exclude(p => p.LedgerAccountYears)
                                                        .Include(p => p.AccountSegments.First().Id)
                                                        .Include(p => p.AccountSegments.First().Level)
                                                        .Include(p => p.AccountSegments.First().SegmentId)
                                                        .Include(p => p.AccountSegments.First().Segment.Code)
                                                        .Include(p => p.AccountSegments.First().Segment.Name)
                                                        .Include(p => p.ClosingAccount.AccountNumber)
                                                        .Include(p => p.ClosingAccount.Name)
                                                        .Exclude(p => p.Budgets.First().Account)
                                                        .Include(p => p.Group1.Category)
                                                        .Include(p => p.Group1.Name)
                                                        .Include(p => p.Group2.Name)
                                                        .Include(p => p.Group3.Name)
                                                        .Include(p => p.Group4.Name)
                                                        .Include(p => p.AccountBalances.First().PeriodNumber)
                                                        .Include(p => p.AccountBalances.First().DebitCredit)
                                                        .Include(p => p.AccountBalances.First().TotalAmount)
                                                        );

        protected override Expression<Func<Account, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<Account, object>>[]
            {
                p => p.AccountSegments.First().Segment,
                p => p.Budgets,
                p => p.ClosingAccount,
                p => p.Group1,
                p => p.Group2,
                p => p.Group3,
                p => p.Group4,
                p => p.AccountBalances
            };
        }

        public AccountController()
            : base(new AccountService())
        {
            _glAccount = new ServiceBase<GLAccountSelection>();
            _ledger = new ServiceBase<Ledger>();
            _fiscalYear = new ServiceBase<FiscalYear>();
        }
        
        [HttpGet]
        [Route("api/v1/fiscalyears/{fiscalYearId}/accounts/lookup/{name}")]
        public IHttpActionResult AccountLookup(Guid fiscalYearId, string name)
        {
            string fields = $"{nameof(GLAccountSelection.Id)},{nameof(GLAccountSelection.AccountNumber)},{nameof(GLAccountSelection.Description)}";

            var search = PageableSearch.Max;
            
            return FinalizeResponse(_glAccount.GetAllWhereExpression((a=> a.FiscalYearId == fiscalYearId && a.AccountNumber.Contains(name)), search), null, search, null, null);
        }

        [HttpGet]
        [Route("api/v1/accounts/{id}", Name = RouteNames.Account + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/accounts/activity/{id}", Name = RouteNames.AccountActivity + RouteVerbs.Get)]
        public IHttpActionResult GetAccountActivity(Guid id)
        {   
            return base.CustomAction(() => Service.GetAccountActivity(id));
        }

        [HttpGet]
        [Route("api/v1/accounts/activity/{id}/detail")]
        public IHttpActionResult GetAccountActivityDetail(Guid id)
        {
            return base.CustomAction(() => Service.GetAccountActivityDetail(id));
        }

        [HttpGet]
        [Route("api/v1/accounts/fiscalyear/{fiscalyearId}/accountnumber/{accountnumber}")]
        public IHttpActionResult GetAccountByAccountNumberFiscalYear(Guid fiscalYearId, string accountNumber)
        {
            try
            {
                return FinalizeResponse(_glAccount.GetAllWhereExpression((a => a.FiscalYearId == fiscalYearId && a.AccountNumber == accountNumber), null), null, null, null, null);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/accounts/fiscalyear/{id}", Name = ROUTENAME_GETALLLEDGERACCOUNTS)]
        public HttpResponseMessage GetAccountSelectorAccounts(Guid Id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var accounts = new GLAccountSelectionService().GetGLAccountsForFiscalYearId(Id);
               // var accounts = _glAccount.GetAllWhereExpression(l => l.FiscalYearId == Id, search);
                return Request.CreateResponse(HttpStatusCode.OK, DataSourceLoader.Load(accounts, loadOptions));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, InternalServerError(new Exception(ex.Message)));
            }
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
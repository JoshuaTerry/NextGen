using DDI.Services.GL;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class AccountController : GenericController<Account>
    {
        protected new IAccountService Service => (IAccountService)base.Service;
        public AccountController(IAccountService service) : base(service) { }
        
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

        [HttpGet]
        [Route("api/v1/fiscalyears/{fiscalYearId}/accounts/lookup/{name}")]
        public IHttpActionResult AccountLookup(Guid fiscalYearId, string name)
        {
            var search = PageableSearch.Max;

            search.OrderBy = OrderByProperties.SortKey;

            var glAccountSelectionService = Factory.CreateService<GLAccountSelectionService>();

            return FinalizeResponse(glAccountSelectionService.GetAllWhereExpression((a=> a.FiscalYearId == fiscalYearId && a.AccountNumber.Contains(name)), search), search, null);
        }

        [HttpGet]
        [Route("api/v1/accounts/fiscalyear/{fiscalYearId}/account/{accountnumber}")]
        public IHttpActionResult ValidateAccountNumber(Guid fiscalYearId, string accountnumber)
        {
            return FinalizeResponse(Service.ValidateAccountNumber(fiscalYearId, accountnumber));
        }

        [HttpGet]
        [Route("api/v1/accounts/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/accounts/activity/{id}")]
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
                var search = PageableSearch.Max;

                IService<GLAccountSelection> _glAccountService = Factory.CreateService<IService<GLAccountSelection>>();

                var accounts = _glAccountService.GetAllWhereExpression(l => l.FiscalYearId == fiscalYearId && l.AccountNumber == accountNumber, search);
                return FinalizeResponse(accounts, search, null);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/accounts/fiscalyear/{id}")]
        public HttpResponseMessage GetAccountSelectorAccounts(Guid Id, DataSourceLoadOptions loadOptions)
        {
            try
            {
                var glAccountSelectionService = Factory.CreateService<GLAccountSelectionService>();
                var accounts = glAccountSelectionService.GetGLAccountsForFiscalYearId(Id);
               
                return Request.CreateResponse(HttpStatusCode.OK, DataSourceLoader.Load(accounts, loadOptions));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, InternalServerError(new Exception(ex.Message)));
            }
        }

        [HttpPost]
        [Route("api/v1/accounts")]
        public override IHttpActionResult Post([FromBody] Account item)
        {
            return base.Post(item);
        }

        [Route("api/v1/accounts/{sourceAccountId}/merge/{destinationAccountId}")]
        public IHttpActionResult Post(Guid sourceAccountId, Guid destinationAccountId)
        {
            var response = Service.Merge(sourceAccountId, destinationAccountId);
            return FinalizeResponse(response);
        }

        [HttpPatch]
        [Route("api/v1/accounts/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/accounts/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
using System;
using System.Web.Http;
using DDI.Services;
using DDI.Services.GL;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class AccountController : GenericController<Account>
    {
        protected new IAccountService Service => (IAccountService)base.Service;
        private ServiceBase<GLAccountSelection> _glAccount;
        private const string ROUTENAME_GETALLLEDGERACCOUNTS = RouteNames.Account + RouteNames.FiscalYear + RouteVerbs.Get;


        public AccountController()
            : base(new AccountService())
        {
            _glAccount = new ServiceBase<GLAccountSelection>();
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
        [Route("api/v1/accounts/fiscalyear/{id}",Name = ROUTENAME_GETALLLEDGERACCOUNTS)]
        public IHttpActionResult GetAllLedgerAccounts(Guid Id, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);

                var accounts = _glAccount.GetAllWhereExpression(l => l.FiscalYearId == Id, search);
                return FinalizeResponse(accounts, ROUTENAME_GETALLLEDGERACCOUNTS, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
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
using DDI.Services;
using DDI.Shared.Models.Client.INV;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;

//commented out code may be used once tables and real data in place
namespace DDI.WebApi.Controllers.INV
{
    public class InvestmentAutomatedTransactionsController : GenericController<InvestmentAutomatedTransaction>
    {

        private InvestmentAutomatedTransactionService _laService;

        public InvestmentAutomatedTransactionsController(InvestmentAutomatedTransactionService service) : base(service)
        {
            _laService = service;
        }

        protected override Expression<Func<InvestmentAutomatedTransaction, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<InvestmentAutomatedTransaction, object>>[]
            {
                a => a.PaymentMethod
            };
        }

        [Authorize] //(Roles = Permissions.INV_Read + "," + Permissions.Settings_Read)]
        [Route("api/v1/investmentautomatedtransactions")]
        public override IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/investmentautomatedtransactions/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            try
            {
                var response = _laService.GetAutomatedTransactionById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            } 
        }

        //[Authorize] //(Roles = Permissions.INV_Read)] //add investment roles when available
        [HttpGet]
        [Route("api/v1/investmentautomatedtransactions/investment/{id}")]
        public IHttpActionResult GetByInvestmentId(Guid id)
        {
            

            try
            {                 
                var response = _laService.GetAutomatedTransactionByInvestmentId(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }


        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpPost]
        [Route("api/v1/investmentautomatedtransactions")]
        public override IHttpActionResult Post([FromBody] InvestmentAutomatedTransaction entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/investmentautomatedtransactions/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize] //(Roles = Permissions.INV_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/investmentautomatedtransactions/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

    }
}
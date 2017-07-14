using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Routing;

namespace DDI.WebApi.Controllers.GL
{

    [Authorize]
    public class FundFromToController : GenericController<FundFromTo>
    {

        protected new IFundFromToService Service => (IFundFromToService)base.Service;
        public FundFromToController(IFundFromToService service) : base(service) { }

        protected override Expression<Func<FundFromTo, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<FundFromTo, object>>[]
            {
                c => c.FiscalYear,
                c => c.FromLedgerAccount,
                c => c.ToLedgerAccount,
                c => c.Fund, 
                c => c.Fund.FundSegment,
                c => c.OffsettingFund,
                c => c.OffsettingFund.FundSegment
            };
        }

        protected override Expression<Func<FundFromTo, object>>[] GetDataIncludesForList() => GetDataIncludesForSingle();

        protected override string FieldsForList => FieldListBuilder
            .Include(p => p.FundId)
            .Include(p => p.Fund.FundSegment.Code)
            .Include(p => p.Fund.FundSegment.Name)
            .Include(p => p.FiscalYearId)
            .Include(p => p.FiscalYear.Name)
            .Include(p => p.CreatedBy)
            .Include(p => p.CreatedOn)
            .Include(p => p.DisplayName)
            .Include(p => p.FromLedgerAccount.AccountNumber)
            .Include(p => p.FromLedgerAccount.Name)
            .Include(p => p.FromLedgerAccountId)
            .Include(p => p.ToLedgerAccount.AccountNumber)
            .Include(p => p.ToLedgerAccount.Name)
            .Include(p => p.ToLedgerAccountId)
            .Include(p => p.OffsettingFund.FundSegment.Code)
            .Include(p => p.OffsettingFund.FundSegment.Name)
            .Include(p => p.OffsettingFundId)
            .Include(p => p.LastModifiedBy)
            .Include(p => p.LastModifiedOn)
            .Include(p => p.Id)
            .Include(p => p.FromAccountId)
            .Include(p => p.ToAccountId);

        protected override string FieldsForAll => FieldsForList;

        [HttpGet]
        [Route("api/v1/fundfromtos/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/fundfromtos/fund/{fundid}")]
        [Route("api/v1/funds/{fundid}/fundfromto")]
        public IHttpActionResult GetByFund(Guid fundid, string fields = null)
        {
            try
            {
                var result = Service.GetForFund(fundid);
                return FinalizeResponse(result, null, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("api/v1/fundfromtos")]
        public override IHttpActionResult Post([FromBody] FundFromTo item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/fundfromtos/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/fundfromtos/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
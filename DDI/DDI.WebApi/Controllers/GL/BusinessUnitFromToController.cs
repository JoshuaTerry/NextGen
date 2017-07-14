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
    public class BusinessUnitFromToController : GenericController<BusinessUnitFromTo>
    {
        protected new IBusinessUnitFromToService Service => (IBusinessUnitFromToService)base.Service;

        public BusinessUnitFromToController(IBusinessUnitFromToService service) : base(service) { }

        protected override Expression<Func<BusinessUnitFromTo, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<BusinessUnitFromTo, object>>[]
            {
                c => c.FromLedgerAccount,
                c => c.ToLedgerAccount,
                c => c.BusinessUnit, 
                c => c.OffsettingBusinessUnit,
                c => c.FiscalYear
            };
        }

        protected override Expression<Func<BusinessUnitFromTo, object>>[] GetDataIncludesForList() => GetDataIncludesForSingle();

        protected override string FieldsForList => FieldListBuilder
            .Include(p => p.BusinessUnitId)
            .Include(p => p.BusinessUnit.Code)
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
            .Include(p => p.OffsettingBusinessUnit.Code)
            .Include(p => p.OffsettingBusinessUnitId)
            .Include(p => p.LastModifiedBy)
            .Include(p => p.LastModifiedOn)
            .Include(p => p.Id)
            .Include(p => p.FromAccountId)
            .Include(p => p.ToAccountId);

        protected override string FieldsForAll => FieldsForList;

        [HttpGet]
        [Route("api/v1/businessunitfromtos/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/businessunitfromtos/fiscalyear/{yearid}")]
        [Route("api/v1/fiscalyears/{yearid}/businessunitfromto")]
        public IHttpActionResult GetByFiscalYear(Guid yearid, string fields = null)
        {
            try
            {
                var result = Service.GetForFiscalYear(yearid);
                return FinalizeResponse(result, null, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("api/v1/businessunitfromtos")]
        public override IHttpActionResult Post([FromBody] BusinessUnitFromTo item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/businessunitfromtos/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/businessunitfromtos/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
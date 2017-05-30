using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Routing;
using DDI.Services.ServiceInterfaces;
using DDI.Services.GL;
using DDI.Shared.Helpers;

namespace DDI.WebApi.Controllers.GL
{

    [Authorize]
    public class BusinessUnitFromToController : GenericController<BusinessUnitFromTo>
    {
        private const string ROUTENAME_GETBYYEAR = RouteNames.FiscalYear + RouteNames.BusinessUnitFromTo + RouteVerbs.Get;

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
        [Route("api/v1/businessunitfromtos/{id}", Name = RouteNames.BusinessUnitFromTo + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/businessunitfromtos/fiscalyear/{yearid}", Name = RouteNames.BusinessUnitFromTo + RouteNames.FiscalYear + RouteVerbs.Get)]
        [Route("api/v1/fiscalyears/{yearid}/businessunitfromto", Name = ROUTENAME_GETBYYEAR)]
        public IHttpActionResult GetByFiscalYear(Guid yearid, string fields = null)
        {
            try
            {
                var result = Service.GetForFiscalYear(yearid);
                return FinalizeResponse(result, ROUTENAME_GETBYYEAR, null, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("api/v1/businessunitfromtos", Name = RouteNames.BusinessUnitFromTo + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] BusinessUnitFromTo item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/businessunitfromtos/{id}", Name = RouteNames.BusinessUnitFromTo + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/businessunitfromtos/{id}", Name = RouteNames.BusinessUnitFromTo + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
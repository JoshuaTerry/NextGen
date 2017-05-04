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

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class FundController : GenericController<Fund>
{
   
        private const string ROUTENAME_FUND = RouteNames.Ledger + RouteNames.Fund + RouteVerbs.Get;

        protected override Expression<Func<Fund, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<Fund, object>>[]
            {
                c => c.FundSegment, 
                c => c.ClosingExpenseAccount,
            };
        }


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
        try {
            var search = new PageableSearch(offset, limit, orderBy);
            var result = Service.GetAllWhereExpression(f => f.FiscalYearId == fiscalyearid);
            //if (result == null)
            //{
            //    NotFound();
            //}

            //return Ok(result);
            return FinalizeResponse(result, "", search, fields);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            return InternalServerError();
        }
     }
        [HttpGet]
        [Route("api/v1/fund/{fiscalyearid}/fiscalyear/FundSegment")]
        public IHttpActionResult GetByFiscalYearbysegment(Guid fiscalyearid, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.Order)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var result = Service.GetAllWhereExpression(f => f.FiscalYearId == fiscalyearid);
                //if (result == null)
                //{
                //    NotFound();
                //}

                //return Ok(result);
                return FinalizeResponse(result, "", search, fields);
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

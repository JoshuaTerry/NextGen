using DDI.Services.GL;
using DDI.Services.Search;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class FiscalYearController : GenericController<FiscalYear>
    {
        private const string ROUTENAME_GETALLBYLEDGER = RouteNames.Ledger + RouteNames.FiscalYear + RouteVerbs.Get;
        private const string ROUTENAME_GETALLBYUNIT = RouteNames.BusinessUnit + RouteNames.FiscalYear + RouteVerbs.Get;

        protected override string FieldsForList => $"{nameof(FiscalYear.Id)},{nameof(FiscalYear.Name)},{nameof(FiscalYear.Status)}";

        [HttpGet]
        [Route("api/v1/fiscalyears/ledger/{ledgerId}", Name = RouteNames.FiscalYear + RouteNames.Ledger + RouteVerbs.Get)]
        [Route("api/v1/ledgers/{ledgerId}/fiscalyears", Name = ROUTENAME_GETALLBYLEDGER)]
        public IHttpActionResult GetAllByLedgerId(Guid ledgerId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = "all")
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var result = _service.GetAllWhereExpression(fy => fy.LedgerId == ledgerId, search);

                return FinalizeResponse(result, ROUTENAME_GETALLBYLEDGER, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/fiscalyears/businessunit/{unitId}", Name = RouteNames.FiscalYear + RouteNames.BusinessUnit + RouteVerbs.Get)]
        [Route("api/v1/businessunits/{unitId}/fiscalyears", Name = ROUTENAME_GETALLBYUNIT)]
        public IHttpActionResult GetAllByBusinessUnitId(Guid unitId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var result = _service.GetAllWhereExpression(fy => fy.Ledger.BusinessUnitId == unitId, search);

                return FinalizeResponse(result, ROUTENAME_GETALLBYUNIT, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/fiscalyears/{id}", Name = RouteNames.FiscalYear + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/fiscalyears", Name = RouteNames.FiscalYear + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] FiscalYear entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/fiscalyears/{id}", Name = RouteNames.FiscalYear + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/fiscalyears/{id}", Name = RouteNames.FiscalYear + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

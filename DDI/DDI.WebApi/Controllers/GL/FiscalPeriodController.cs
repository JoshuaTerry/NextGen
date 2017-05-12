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

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class FiscalPeriodController : GenericController<FiscalPeriod>
    {
        private const string ROUTENAME_GETALLBYYEAR = RouteNames.FiscalYear + RouteNames.FiscalPeriod + RouteVerbs.Get;

        [HttpGet]
        [Route("api/v1/fiscalperiods/fiscalyear/{fiscalYearId}", Name = RouteNames.FiscalPeriod + RouteNames.FiscalYear + RouteVerbs.Get)]
        [Route("api/v1/fiscalyears/{fiscalYearId}/fiscalperiods", Name = ROUTENAME_GETALLBYYEAR)]
        public IHttpActionResult GetAllByFiscalYearId(Guid fiscalYearId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = nameof(FiscalPeriod.PeriodNumber), string fields = null)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var result = Service.GetAllWhereExpression(fp => fp.FiscalYearId == fiscalYearId, search);

                return FinalizeResponse(result, ROUTENAME_GETALLBYYEAR, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/fiscalperiods/{id}", Name = RouteNames.FiscalPeriod + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/fiscalperiods", Name = RouteNames.FiscalPeriod + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] FiscalPeriod entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/fiscalperiods/{id}", Name = RouteNames.FiscalPeriod + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/fiscalperiods/{id}", Name = RouteNames.FiscalPeriod + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

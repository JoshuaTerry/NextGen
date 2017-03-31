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
    public class FiscalPeriodController : GenericController<FiscalPeriod>
    {
        [HttpGet]
        [Route("api/v1/fiscalperiod/fiscalyear/{fiscalYearId}")]
        public IHttpActionResult GetAllByFiscalYearId(Guid fiscalYearId)
        {
            try
            {
                var search = new PageableSearch(SearchParameters.OffsetDefault, SearchParameters.LimitDefault, "PeriodNumber");
                var result = _service.GetAllWhereExpression(fp => fp.FiscalYearId == fiscalYearId, search);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/fiscalperiod/{id}", Name = RouteNames.FiscalPeriod + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/fiscalperiod", Name = RouteNames.FiscalPeriod + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] FiscalPeriod entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/fiscalperiod/{id}", Name = RouteNames.FiscalPeriod + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/fiscalperiod/{id}", Name = RouteNames.FiscalPeriod + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

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
    public class FiscalYearController : GenericController<FiscalYear>
    {
        [HttpGet]
        [Route("api/v1/fiscalyear/ledger/{ledgerId}")]
        public IHttpActionResult GetAllByLedgerId(Guid ledgerId)
        {
            try
            {
                var result = _service.GetAllWhereExpression(fy => fy.LedgerId == ledgerId);

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
        [Route("api/v1/fiscalyear/{id}", Name = RouteNames.FiscalYear + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/fiscalyear", Name = RouteNames.FiscalYear + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] FiscalYear entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/fiscalyear/{id}", Name = RouteNames.FiscalYear + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/fiscalyear/{id}", Name = RouteNames.FiscalYear + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.GL
{
    public class LedgerController : GeneralLedgerController<Ledger>
    {

        [HttpGet]
        [Route("api/v1/ledgers/{id}", Name = RouteNames.Ledger + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/ledgers/businessunit/{id}", Name = RouteNames.Ledger)]
        public IHttpActionResult GetByBusinessUnit(Guid id, string fields = null)
        {
            var response = Service.GetAllWhereExpression(l => l.BusinessUnitId == id);
            return Ok(response);
        }

        [HttpPost]
        [Route("api/v1/ledgers", Name = RouteNames.Ledger + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Ledger item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/ledgers/{id}", Name = RouteNames.Ledger + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/ledgers/{id}", Name = RouteNames.Ledger + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
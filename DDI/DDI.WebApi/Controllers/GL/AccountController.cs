using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.GL
{
    [Authorize]
    public class AccountController : GeneralLedgerController<Account>
    {
        [HttpGet]
        [Route("api/v1/businessunit/{businessUnitId}/accounts", Name = RouteNames.Account)]
        public IHttpActionResult GetAll(Guid businessUnitId, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Account, businessUnitId, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/accounts/{id}", Name = RouteNames.Account + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/accounts", Name = RouteNames.Account + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Account item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/accounts/{id}", Name = RouteNames.Account + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/accounts/{id}", Name = RouteNames.Account + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
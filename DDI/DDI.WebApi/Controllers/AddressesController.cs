using System.Runtime.InteropServices;
using DDI.Services;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    //[Authorize]
    public class AddressesController : ControllerBase<Address>
    {
        [HttpGet]
        [Route("api/v1/addresses", Name = RouteNames.Address)]
        public IHttpActionResult GetAll(int? limit = 25, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Address, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/addresses/{id}", Name = RouteNames.Address + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/addresses", Name = RouteNames.Address + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Address item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/addresses/{id}", Name = RouteNames.Address + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/addresses/{id}", Name = RouteNames.Address + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

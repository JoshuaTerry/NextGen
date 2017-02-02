using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    public class ClergyTypesController : ControllerBase<ClergyType>
    {
        [HttpGet]
        [Route("api/v1/clergytypes", Name = RouteNames.ClergyType)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ClergyType, limit, offset, orderBy, fields);
        }

        [HttpPost]
        [Route("api/v1/clergytypes", Name = RouteNames.ClergyType + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ClergyType item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/clergytypes/{id}", Name = RouteNames.ClergyType + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }
    }
}
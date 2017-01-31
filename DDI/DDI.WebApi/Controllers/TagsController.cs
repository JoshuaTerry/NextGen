using System;
using System.Drawing;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class TagsController : ControllerBase<Tag>
    {
        [HttpGet]
        [Route("api/v1/tags", Name = RouteNames.Tag)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Tag, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/tags/{id}", Name = RouteNames.Tag + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/tags", Name = RouteNames.Tag + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Tag entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/tags/{id}", Name = RouteNames.Tag + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/tags/{id}", Name = RouteNames.Tag + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
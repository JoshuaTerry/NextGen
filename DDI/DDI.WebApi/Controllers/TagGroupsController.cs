using System;
using System.Drawing;
using System.Linq;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class TagGroupsController : ControllerBase<TagGroup>
    {
        [HttpGet]
        [Route("api/v1/taggroups", Name = RouteNames.TagGroup)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.Order, string fields = null)
        {
            return base.GetAll(RouteNames.Tag, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/taggroups/{id}", Name = RouteNames.TagGroup + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/taggroups", Name = RouteNames.TagGroup + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] TagGroup entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/taggroups/{id}", Name = RouteNames.TagGroup + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/taggroups/{id}", Name = RouteNames.TagGroup + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
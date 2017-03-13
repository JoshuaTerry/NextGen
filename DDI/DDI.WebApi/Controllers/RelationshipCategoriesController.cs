using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class RelationshipCategoriesController : ControllerBase<RelationshipCategory>
    {
        [HttpGet]
        [Route("api/v1/relationshipcategories", Name = RouteNames.RelationshipCategories)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.RelationshipCategories, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/relationshipcategories/{id}", Name = RouteNames.RelationshipCategories + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/relationshipcategories", Name = RouteNames.RelationshipCategories + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] RelationshipCategory entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/relationshipcategories/{id}", Name = RouteNames.RelationshipCategories + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/relationshipcategories/{id}", Name = RouteNames.RelationshipCategories + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

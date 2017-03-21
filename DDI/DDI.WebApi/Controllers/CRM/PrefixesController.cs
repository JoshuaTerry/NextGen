using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    public class PrefixesController : GenericController<Prefix>
    {

        protected override Expression<Func<Prefix, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<Prefix, object>>[]
            {
                e => e.Gender
            };
        }

        [HttpGet]
        [Route("api/v1/prefixes", Name = RouteNames.Prefix)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Prefix, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/prefixes/{id}", Name = RouteNames.Prefix + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/prefixes", Name = RouteNames.Prefix + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Prefix entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/prefixes/{id}", Name = RouteNames.Prefix + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/prefixes/{id}", Name = RouteNames.Prefix + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

    }
}
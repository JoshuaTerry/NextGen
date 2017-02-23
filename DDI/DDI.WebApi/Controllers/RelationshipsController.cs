using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using DDI.Services.ServiceInterfaces;

namespace DDI.WebApi.Controllers
{
    public class RelationshipsController : ControllerBase<Relationship>
    {
        private const string DEFAULT_FIELDS = "Id,RelationshipType.RelationshipCategory.DisplayName,RelationshipType.RelationshipCategory.IsShownInQuickView,Constituent1.Id,Constituent1.Links.Self,Constituent2.Id,Constituent2.Links.Self,DisplayName,Links";
        protected override Expression<Func<Relationship, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<Relationship, object>>[]
            {
                r => r.RelationshipType.RelationshipCategory,
                c => c.Constituent1,
                c => c.Constituent2
            };
        }

        [HttpGet]
        [Route("api/v1/relationships", Name = RouteNames.Relationship)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitDefault, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = DEFAULT_FIELDS)
        {
            return base.GetAll(RouteNames.Relationship, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/relationships/{id}", Name = RouteNames.Relationship + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/relationships", Name = RouteNames.Relationship + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Relationship entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/relationships/{id}", Name = RouteNames.Relationship + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/relationships/{id}", Name = RouteNames.Relationship + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/relationships/constituents/{id}")]
        [Route("api/v1/constituents/{id}/relationships", Name = RouteNames.Constituent + RouteNames.Relationship)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = DEFAULT_FIELDS, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response1 = Service.GetAllWhereExpression(a => a.Constituent1Id == id, search);
                var response2 = Service.GetAllWhereExpression(a => a.Constituent2Id == id, search);
                response1.Data = response1.Data.Union(response2.Data).ToList();
                return FinalizeResponse(response1, RouteNames.Constituent + RouteNames.Relationship, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError();
            }
        }
    }
}
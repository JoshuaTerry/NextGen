using DDI.Services.Search;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    public class EducationsController : GenericController<Education>
    {
        protected override Expression<Func<Education, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<Education, object>>[]
            {
                e => e.School,
                e => e.Degree
            };
        }

        [HttpGet]
        [Route("api/v1/educations", Name = RouteNames.Education)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Education, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/educations/{id}", Name = RouteNames.Education + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/educations", Name = RouteNames.Education + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Education entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/educations/{id}", Name = RouteNames.Education + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/educations/{id}", Name = RouteNames.Education + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/educations/constituents/{id}")]
        [Route("api/v1/constituents/{id}/educations", Name = RouteNames.Constituent + RouteNames.Education)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.Education, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}
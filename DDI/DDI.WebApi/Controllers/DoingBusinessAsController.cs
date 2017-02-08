using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class DoingBusinessAsController : ControllerBase<DoingBusinessAs>
    {
        [HttpGet]
        [Route("api/v1/doingbusinessas", Name = RouteNames.DoingBusinessAs)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.DoingBusinessAs, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/doingbusinessas/{id}", Name = RouteNames.DoingBusinessAs + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/doingbusinessas", Name = RouteNames.DoingBusinessAs + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] DoingBusinessAs entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/doingbusinessas/{id}", Name = RouteNames.DoingBusinessAs + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/doingbusinessas/{id}", Name = RouteNames.DoingBusinessAs + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/doingbusinessas/constituents/{id}")]
        [Route("api/v1/constituents/{id}/doingbusinessas", Name = RouteNames.Constituent + RouteNames.DoingBusinessAs)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.DoingBusinessAs, search, fields);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }
    }
}
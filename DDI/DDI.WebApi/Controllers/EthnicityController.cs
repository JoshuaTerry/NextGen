using System;
using System.Linq;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class EthnicityController : ControllerBase<Ethnicity>
    {
        [HttpGet]
        [Route("api/v1/ethnicities", Name = RouteNames.Ethnicity)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Ethnicity, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/ethnicities/{id}", Name = RouteNames.Ethnicity + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/ethnicities", Name = RouteNames.Ethnicity + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Ethnicity entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/ethnicities/{id}", Name = RouteNames.Ethnicity + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/ethnicities/{id}", Name = RouteNames.Ethnicity + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/ethnicities/constituents/{id}")]
        [Route("api/v1/constituents/{id}/ethnicities", Name = RouteNames.Constituent + RouteNames.Ethnicity)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.Constituents.Any(c => c.Id == id), search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.Ethnicity, search, fields);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }
    }
}
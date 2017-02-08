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
    public class DenominationsController : ControllerBase<Denomination>
    {
        [HttpGet]
        [Route("api/v1/denominations", Name = RouteNames.Denomination)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Denomination, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/denominations/{id}", Name = RouteNames.Denomination + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/denominations", Name = RouteNames.Denomination + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] Denomination entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/denominations/{id}", Name = RouteNames.Denomination + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/denominations/{id}", Name = RouteNames.Denomination + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/denominations/constituents/{id}")]
        [Route("api/v1/constituents/{id}/denominations", Name = RouteNames.Constituent + RouteNames.Denomination)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.Constituents.Any(c => c.Id == id), search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.Denomination, search, fields);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }
    }
}
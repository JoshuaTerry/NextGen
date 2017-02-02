using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using DDI.Services.ServiceInterfaces;

namespace DDI.WebApi.Controllers
{
    public class AlternateIdsController : ControllerBase<AlternateId>
    {

        [HttpGet]
        [Route("api/v1/alternateids", Name = RouteNames.AlternateId)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.AlternateId, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/alternateids/{id}", Name = RouteNames.AlternateId + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/alternateids", Name = RouteNames.AlternateId + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] AlternateId entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/alternateids/{id}", Name = RouteNames.AlternateId + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/alternateids/{id}", Name = RouteNames.AlternateId + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [HttpGet]
        [Route("api/v1/alternateids/constituents/{id}")]
        [Route("api/v1/constituents/{id}/alternateids", Name = RouteNames.Constituent + RouteNames.AlternateId)]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = null, int? limit = 25, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.AlternateId, search, fields);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }
    }
}
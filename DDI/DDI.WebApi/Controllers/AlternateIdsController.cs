using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using DDI.Services.ServiceInterfaces;
using DDI.Services.Services;

namespace DDI.WebApi.Controllers
{
    public class AlternateIdsController : ControllerBase<AlternateId>
    {
        public AlternateIdsController()
            : base(new AlternateIdService())
        {
        }

        [HttpGet]
        [Route("api/v1/alternateids", Name = RouteNames.AlternateId)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.AlternateId, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/alternateids/{id}", Name = RouteNames.AlternateId + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(GetUrlHelper(), id, fields);
        }

        [HttpGet]
        [Route("api/v1/alternateids/constituents/{constituentId}")]
        [Route("api/v1/constituents/{constituentId}/alternateids", Name = RouteNames.Constituent + RouteNames.AlternateId)]
        public IHttpActionResult GetByConstituentId(Guid constituentId, string fields = null)
        {
            try
            {
                var response = ((AlternateIdService)Service).GetAlternateIdsByConstituent(constituentId);
                if (response.Data == null)
                {
                    return NotFound();
                }
                if (!response.IsSuccessful)
                {
                    return BadRequest(response.ErrorMessages.ToString());
                }

                var dynamicResponse = DynamicTransmogrifier.ToDynamicResponse(response, GetUrlHelper(), fields);

                return Ok(dynamicResponse);
            }
            catch (Exception ex)
            {
                LoggerBase.Error(ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/v1/alternateids", Name = RouteNames.AlternateId + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] AlternateId entityToSave)
        {
            return base.Post(GetUrlHelper(), entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/alternateids/{id}", Name = RouteNames.AlternateId + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(GetUrlHelper(), id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/alternateids/{id}", Name = RouteNames.AlternateId + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
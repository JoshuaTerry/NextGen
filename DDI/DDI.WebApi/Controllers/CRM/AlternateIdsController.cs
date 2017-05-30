using System;
using System.Web.Http;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class AlternateIdsController : GenericController<AlternateId>
    {
        public AlternateIdsController(IService<AlternateId> service) : base(service) { }

        protected override string FieldsForList => $"{nameof(AlternateId.Id)},{nameof(AlternateId.Name)}";

        [HttpGet]
        [Route("api/v1/alternateids", Name = RouteNames.AlternateId)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
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
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search);
                return FinalizeResponse(response, RouteNames.Constituent + RouteNames.AlternateId, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}
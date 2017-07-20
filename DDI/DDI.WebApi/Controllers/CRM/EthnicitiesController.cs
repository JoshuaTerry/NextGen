using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class EthnicitiesController : GenericController<Ethnicity>
    {
        public EthnicitiesController(IService<Ethnicity> service) : base(service) { }

        protected override string FieldsForList => FieldLists.CodeFields;

        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.Constituents);

        protected new IEthnicitiesService Service => (IEthnicitiesService) base.Service;
   
        [HttpGet]
        [Route("api/v1/ethnicities")]
        public override IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/ethnicities/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/ethnicities")]
        public override IHttpActionResult Post([FromBody] Ethnicity entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/ethnicities/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/ethnicities/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [Authorize(Roles = Permissions.CRM_Read)]
        [HttpGet]
        [Route("api/v1/ethnicities/constituents/{id}")]
        [Route("api/v1/constituents/{id}/ethnicities")]  //Only the routename that matches the Model needs to be defined so that HATEAOS can create the link
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var response = Service.GetAllWhereExpression(a => a.Constituents.Any(c => c.Id == id), search, fields);
                return FinalizeResponse(response, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }

        [Authorize(Roles = Permissions.CRM_ReadWrite)]
        [HttpPost]
        [Route("api/v1/constituents/{id}/ethnicities")]
        public IHttpActionResult AddEthnicitiesToConstituent(Guid id, [FromBody] JObject ethnicityIds)
        {
            try
            {
                var response = Service.AddEthnicitiesToConstituent(id, ethnicityIds);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }
    }
}
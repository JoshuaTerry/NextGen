using DDI.Services.Search;
using DDI.Shared;
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

        public EducationsController(IService<Education> service) : base(service) { }

        protected override Expression<Func<Education, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<Education, object>>[]
            {
                e => e.School,
                e => e.Degree
            };
        }

        protected override string FieldsForList => $"{nameof(Education.Id)},{nameof(Education.Major)},{nameof(Education.DegreeOther)},{nameof(Education.SchoolOther)},{nameof(Education.StartDate)},{nameof(Education.EndDate)}";

        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.Constituent);

        [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
        [HttpGet]
        [Route("api/v1/educations")]
        public override IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
        [HttpGet]
        [Route("api/v1/educations/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/educations")]
        public override IHttpActionResult Post([FromBody] Education entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/educations/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/educations/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [Authorize(Roles = Permissions.CRM_Read)]
        [HttpGet]
        [Route("api/v1/educations/constituents/{id}")]
        [Route("api/v1/constituents/{id}/educations")]
        public IHttpActionResult GetByConstituentId(Guid id, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var response = Service.GetAllWhereExpression(a => a.ConstituentId == id, search, fields);
                return FinalizeResponse(response, search, fields);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}
using DDI.Services;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Routing;


namespace DDI.WebApi.Controllers.CRM
{
    [Authorize(Roles = Permissions.CRM_Settings_Read + "," + Permissions.Settings_Read)]
    public class ConstituentTypesController : GenericController<ConstituentType>
    {
        protected new IConstituentTypeService Service => (IConstituentTypeService)base.Service;

        public ConstituentTypesController(IConstituentTypeService service) : base(service) { }

        protected override Expression<Func<ConstituentType, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<ConstituentType, object>>[]
            {
                a => a.Tags
            };
        }

        protected override Expression<Func<ConstituentType, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<ConstituentType, object>>[]
            {
                c => c.Tags
            };
        }

        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/constituenttypes")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/constituenttypes/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/constituenttypes")]
        public IHttpActionResult Post([FromBody] ConstituentType item)
        {
            return base.Post(item);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/constituenttypes/{id}")]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/constituenttypes/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/constituenttypes/{id}/constituenttypetags")]
        public IHttpActionResult AddTagsToConstituentType(Guid id, [FromBody] JObject tags)
        {
            try
            {
                var constituentType = Service.GetById(id).Data;

                if (constituentType == null)
                {
                    return NotFound();
                }

                var response = Service.AddTagsToConstituentType(constituentType, tags);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(ex);
            }
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/constituenttypes/{id}/tag/{tagId}")]
        public IHttpActionResult RemoveTagFromConstituentType(Guid id, Guid tagId)
        {
            try
            {
                var constituentType = Service.GetById(id).Data;

                if (constituentType == null)
                {
                    return NotFound();
                }

                var response = Service.RemoveTagFromConstituentType(constituentType, tagId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(ex);
            }
        }

    }
}
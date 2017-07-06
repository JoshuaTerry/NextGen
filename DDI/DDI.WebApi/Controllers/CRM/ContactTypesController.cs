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
    [Authorize]
    public class ContactTypesController : GenericController<ContactType>
    {
        public ContactTypesController(IService<ContactType> service) : base(service) { }

        protected override Expression<Func<ContactType, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<ContactType, object>>[]
            {
                a => a.ContactCategory
            };
        }

        protected override Expression<Func<ContactType, object>>[] GetDataIncludesForSingle() => GetDataIncludesForList();

        protected override string FieldsForList => FieldLists.CodeFields;

        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.ContactCategoryDefaults).Exclude(p => p.ContactCategory.ContactTypes).Exclude(p => p.ContactCategory.DefaultContactType);

        [HttpGet]
        [Route("api/v1/contacttypes")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/contacttypes/newconstituent")]
        public IHttpActionResult GetForNewConstituent()
        {
            try
            {
                string fields = $"{nameof(ContactType.Id)},{nameof(ContactType.Name)},{nameof(ContactType.ContactCategory)}.{nameof(ContactCategory.Code)}";
                var search = PageableSearch.Max;
                var result = Service.GetAllWhereExpression(ct => ct.IsAlwaysShown == true, search, fields);
                return FinalizeResponse(result, search, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/contacttypes/category/{categoryid}")]
        public IHttpActionResult GetByCategoryId(Guid categoryid, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var result = Service.GetAllWhereExpression(ct => ct.ContactCategoryId == categoryid, search, fields);
                return FinalizeResponse(result, search, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/contacttypes/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/contacttypes")]
        public IHttpActionResult Post([FromBody] ContactType entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/contacttypes/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/contacttypes/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
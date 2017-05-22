using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Services.Search;
using DDI.Shared.Helpers;
using DDI.Services;
using DDI.Shared;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class ContactTypesController : GenericController<ContactType>
    {
        public ContactTypesController() : base(Factory.CreateService<ContactTypeService>()) { }

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
        [Route("api/v1/contacttypes", Name = RouteNames.ContactType)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ContactType, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/contacttypes/newconstituent", Name = RouteNames.ContactType + RouteNames.Constituent)]
        public IHttpActionResult GetForNewConstituent()
        {
            try
            {
                var result = Service.GetAllWhereExpression(ct => ct.IsAlwaysShown == true);
                var search = PageableSearch.Max;
                string fields = $"{nameof(ContactType.Id)},{nameof(ContactType.Name)},{nameof(ContactType.ContactCategory)}.{nameof(ContactCategory.Code)}";
                return FinalizeResponse(result, RouteNames.ContactType + RouteNames.Constituent, search, fields);                
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
                var result = Service.GetAllWhereExpression(ct => ct.ContactCategoryId == categoryid);
                var search = new PageableSearch(offset, limit, orderBy);
                return FinalizeResponse(result, string.Empty, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/contacttypes/{id}", Name = RouteNames.ContactType + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/contacttypes", Name = RouteNames.ContactType + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ContactType entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/contacttypes/{id}", Name = RouteNames.ContactType + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/contacttypes/{id}", Name = RouteNames.ContactType + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
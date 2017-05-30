using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Shared;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class ContactInfoController : GenericController<ContactInfo>
    {
        public ContactInfoController(IService<ContactInfo> service) : base(service) { }

        protected override Expression<Func<ContactInfo, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<ContactInfo, object>>[]
            {
                c => c.ContactType
            };
        }

        protected override Expression<Func<ContactInfo, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<ContactInfo, object>>[]
            {
               c => c.ContactType
            };
        }

        [HttpGet]
        [Route("api/v1/contactinfo", Name = RouteNames.ContactInfo)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ContactInfo, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/contactinfo/{id}", Name = RouteNames.ContactInfo + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/contactinfo/{categoryid}/{constituentid}", Name = RouteNames.ContactCategory + RouteNames.ContactInfo)]
        public IHttpActionResult GetContactInfoByContactCategoryForConstituent(Guid? categoryId, Guid? constituentId)
        {
            try
            {
                var result = Service.GetAllWhereExpression(c => c.ConstituentId == constituentId && c.ContactType.ContactCategoryId == categoryId);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPost]
        [Route("api/v1/contactinfo", Name = RouteNames.ContactInfo + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ContactInfo entityToSave)
        {
            return base.Post(entityToSave);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/contactinfo/{id}", Name = RouteNames.ContactInfo + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [Authorize(Roles = Permissions.CRM_Settings_ReadWrite + "," + Permissions.Settings_ReadWrite)]
        [HttpDelete]
        [Route("api/v1/contactinfo/{id}", Name = RouteNames.ContactInfo + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
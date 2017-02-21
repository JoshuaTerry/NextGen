using System;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class ContactInfoController : ControllerBase<ContactInfo>
    {
        protected new IContactInfoService Service => (IContactInfoService)base.Service;

        public ContactInfoController()
            : base(new ContactInfoService())
        {
        }

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
            var result = Service.GetContactInfoByContactCategoryForConstituent(categoryId, constituentId);

            if (result == null)
            {
                return NotFound();
            }

            if (!result.IsSuccessful)
            {
                return InternalServerError();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("api/v1/contactinfo", Name = RouteNames.ContactInfo + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ContactInfo entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/contactinfo/{id}", Name = RouteNames.ContactInfo + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/contactinfo/{id}", Name = RouteNames.ContactInfo + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
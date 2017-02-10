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
    public class ContactCategoryController : ControllerBase<ContactCategory>
    {

        [HttpGet]
        [Route("api/v1/contactcategory", Name = RouteNames.ContactCategory)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ContactInfo, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/contactcategory/{id}", Name = RouteNames.ContactCategory + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

       

        [HttpPost]
        [Route("api/v1/contactcategory", Name = RouteNames.ContactCategory + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] ContactCategory entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/contactcategory/{id}", Name = RouteNames.ContactCategory + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/contactcategory/{id}", Name = RouteNames.ContactCategory + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }

}


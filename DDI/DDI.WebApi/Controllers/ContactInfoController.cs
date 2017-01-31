using System;
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
        [HttpGet]
        [Route("api/v1/contactinfo", Name = RouteNames.ContactInfo)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ContactInfo, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/contactinfo/{id}", Name = RouteNames.ContactInfo + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
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
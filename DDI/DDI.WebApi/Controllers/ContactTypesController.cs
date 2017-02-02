using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;

namespace DDI.WebApi.Controllers
{
    public class ContactTypesController : ControllerBase<ContactType>
    {
        [HttpGet]
        [Route("api/v1/contacttypes", Name = RouteNames.ContactType)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.ContactType, limit, offset, orderBy, fields);
        }
    }
}
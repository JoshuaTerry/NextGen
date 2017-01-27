using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class MaritalStatusesController : ControllerBase<MaritalStatus>
    {
        [HttpGet]
        [Route("api/v1/maritalstatuses", Name = RouteNames.MaritalStatus)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.MaritalStatus, limit, offset, orderBy, fields);
        }
    }
}
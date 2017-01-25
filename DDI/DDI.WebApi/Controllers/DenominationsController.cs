using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class DenominationsController : ControllerBase<Denomination>
    {
        [HttpGet]
        [Route("api/v1/denominations", Name = RouteNames.Denomination)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderby = "DisplayName", string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.Denomination, limit, offset, orderby, fields);
        }
    }
}
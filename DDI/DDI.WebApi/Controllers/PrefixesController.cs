using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class PrefixesController : ControllerBase<Prefix>
    {
        [HttpGet]
        [Route("api/v1/prefixes", Name = RouteNames.Prefix)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderby = "DisplayName", string fields = null)
        {
            return base.GetAll(RouteNames.Prefix, limit, offset, orderby, fields);
        }
    }
}
using System.Web.Http;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class LanguagesController : ControllerBase<Language>
    {
        [HttpGet]
        [Route("api/v1/languages", Name = RouteNames.Language)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderby = "DisplayName", string fields = null)
        {
            return base.GetAll(RouteNames.Language, limit, offset, orderby, fields);
        }
    }
}
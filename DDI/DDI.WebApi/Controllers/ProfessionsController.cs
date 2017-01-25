using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using System.Web.Http;
using System.Web.Mvc.Html;
using DDI.Services.Search;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class ProfessionsController : ControllerBase<Profession>
    {
        [HttpGet]
        [Route("api/v1/professions", Name = RouteNames.Profession)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = "DisplayName", string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.Profession, limit, offset, orderBy, fields);
        }
    }
}
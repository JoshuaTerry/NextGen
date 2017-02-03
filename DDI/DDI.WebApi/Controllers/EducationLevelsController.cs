using System.Drawing;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class EducationLevelsController : ControllerBase<EducationLevel>
    {
        [HttpGet]
        [Route("api/v1/educationlevels", Name = RouteNames.EducationLevel)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.EducationLevel, limit, offset, orderBy, fields);
        }
    }
}
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class GendersController : ControllerBase<Gender>
    {
        [HttpGet]
        [Route("api/v1/genders", Name = RouteNames.Gender)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Gender, limit, offset, orderBy, fields);
        }
    }
}
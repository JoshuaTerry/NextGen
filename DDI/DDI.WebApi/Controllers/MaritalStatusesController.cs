using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class MaritalStatusesController : ControllerBase<MaritalStatus>
    {
        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/maritalstatuses", Name = RouteNames.MaritalStatus)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.MaritalStatus, limit, offset, orderBy, fields);
        }
    }
}
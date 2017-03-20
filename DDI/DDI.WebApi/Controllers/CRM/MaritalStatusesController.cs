using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    public class MaritalStatusesController : GenericController<MaritalStatus>
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
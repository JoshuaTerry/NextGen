using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    public class GendersController : GenericController<Gender>
    {
        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/genders", Name = RouteNames.Gender)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.Gender, limit, offset, orderBy, fields);
        }
    }
}
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    public class AddressTypesController : GenericController<AddressType>
    {
        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/addresstypes", Name = RouteNames.AddressType)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.AddressType, limit, offset, orderBy, fields);
        }
    }
}
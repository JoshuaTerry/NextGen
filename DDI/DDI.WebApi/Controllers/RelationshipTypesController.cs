using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class RelationshipTypesController : ControllerBase<RelationshipType>
    {
        [HttpGet]
        [Route("api/v1/relationshiptypes", Name = RouteNames.RelationshipTypes)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.RelationshipTypes, limit, offset, orderBy, fields);
        }
    }
}
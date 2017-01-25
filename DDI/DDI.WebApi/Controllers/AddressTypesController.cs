using System.Data.Entity.Core.Common.CommandTrees;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using LinqKit;

namespace DDI.WebApi.Controllers
{
    public class AddressTypesController : ControllerBase<AddressType>
    {
        [HttpGet]
        [Route("api/v1/addresstypes", Name = RouteNames.AddressType)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = "DisplayName", string fields = null)
        {
            return base.GetAll(GetUrlHelper(), RouteNames.AddressType, limit, offset, orderBy, fields);
        }
    }
}
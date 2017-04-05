using DDI.Shared.Statics;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class BusinessUnitControllers : ApiController
    {
        [HttpGet]
        [Route("api/v1/businessunit", Name = RouteNames.BusinessUnit)]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
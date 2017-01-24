using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class MaritalStatusesController : ControllerBase
    {
        ServiceBase<MaritalStatus> _service;

        public MaritalStatusesController() : this(new ServiceBase<MaritalStatus>()) { }
        internal MaritalStatusesController(ServiceBase<MaritalStatus> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/maritalstatuses", Name = RouteNames.MaritalStatus)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderby = "DisplayName", string fields = null)
        {
            var search = new PageableSearch()
            {
                Limit = limit,
                Offset = offset,
                OrderBy = orderby
            };

            try
            {
                var result = _service.GetAll(search);

                if (result == null)
                {
                    return NotFound();
                }
                if (!result.IsSuccessful)
                {
                    return InternalServerError();
                }

                var totalCount = result.TotalResults;

                Pagination.AddPaginationHeaderToResponse(GetUrlHelper(), search, totalCount, RouteNames.MaritalStatus);
                var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper(), fields);

                return Ok(dynamicResult);
            }
            catch (System.Exception)
            {
                return InternalServerError();
            }
        }
    }
}
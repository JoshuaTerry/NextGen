using System.Web.Http;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;

namespace DDI.WebApi.Controllers
{
    public class LanguagesController : ControllerBase
    {
        ServiceBase<Language> _service;

        public LanguagesController() : this(new ServiceBase<Language>()) { }
        internal LanguagesController(ServiceBase<Language> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("api/v1/languages", Name = RouteNames.Language)]
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

                Pagination.AddPaginationHeaderToResponse(GetUrlHelper(), search, totalCount, RouteNames.Language);
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
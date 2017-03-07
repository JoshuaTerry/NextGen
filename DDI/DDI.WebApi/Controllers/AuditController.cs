using DDI.Services;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace DDI.WebApi.Controllers
{
    public class AuditController : ApiController
    {
        private AuditService _service = null;

        public AuditController()
        {
            _service = new AuditService();
        }

        [HttpGet]
        [Route("api/v1/audit/{id}", Name = RouteNames.Audit)]
        public IHttpActionResult GetByConstituentId(Guid id, DateTime? start = null, DateTime? end = null, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                start = start ?? DateTime.Now.AddDays(-90);
                end = end ?? DateTime.Now;

                var search = new PageableSearch(offset, limit, orderBy);

                //var response = _service.GetAllWhereExpression(a => a.ObjectChanges.Where(o => o.ObjectReference == id.ToString()), search);

                //var response = _service.GetAll(id, start.Value, end.Value, search);
                //return FinalizeResponse(response, RouteNames.Audit, search);

                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private IHttpActionResult FinalizeResponse<T1>(IDataResponse<List<T1>> response, string routeName, IPageable search) where T1 : class
        {
            try
            {
                var urlHelper = new UrlHelper(Request);
                if (!response.IsSuccessful)
                {
                    return BadRequest(response.ErrorMessages.ToString());
                }

                var totalCount = response.TotalResults;
                var pager = new Pagination();
                var transform = new DynamicTransmogrifier();
                pager.AddPaginationHeaderToResponse(urlHelper, search, totalCount, routeName);
                var dynamicResponse = transform.ToDynamicResponse(response, null);

                return Ok(dynamicResponse);
            }
            catch (Exception ex)
            { 
                return InternalServerError();
            }
        }
    }
}
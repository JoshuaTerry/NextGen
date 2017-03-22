using DDI.Logger;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Routing;

namespace DDI.WebApi.Controllers.General
{
    public class AuditController : ApiController
    {
        private AuditService _service = null;
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(AuditController));
        protected ILogger Logger => _logger;


        public AuditController()
        {
            _service = new AuditService();
        }

        [HttpGet]
        [Route("api/v1/audit/flat/{id}", Name = "AuditFlat")]
        public IHttpActionResult GetAuditInfo(Guid id, DateTime? start = null, DateTime? end = null, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                start = start ?? DateTime.Today.AddDays(-90);
                end = end ?? DateTime.Today.AddDays(1); 

                var search = new PageableSearch(offset, limit, orderBy);
                var response = _service.GetAllFlat(id, start.Value, end.Value, search);
                  
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/audit/{id}", Name = RouteNames.Audit)]
        public IHttpActionResult GetAuditFlat(Guid id, DateTime? start = null, DateTime? end = null, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                start = start ?? DateTime.Today.AddDays(-90);
                end = end ?? DateTime.Today.AddDays(1);

                var search = new PageableSearch(offset, limit, orderBy);
                var response = _service.GetChanges(id, start.Value, end.Value);
                  
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        private IHttpActionResult FinalizeResponse<T1>(IDataResponse<List<T1>> response, string routeName, IPageable search) where T1 : class
        {
            try
            {
                var urlHelper = new UrlHelper(Request);
                if (!response.IsSuccessful)
                {
                    _logger.LogError(response.ErrorMessages.ToString());
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
                _logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    }
}
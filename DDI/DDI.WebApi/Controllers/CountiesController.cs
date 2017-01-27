using DDI.Services;
using DDI.Services.Search;
using System;
using System.Web.Http;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using Microsoft.AspNet.Identity;

namespace DDI.WebApi.Controllers
{
    public class CountiesController : ControllerBase<County>
    {
        #region Private Fields

        private CountyService _service;

        #endregion Private Fields

        #region Public Constructors

        public CountiesController()
            : this(new CountyService())
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal CountiesController(CountyService service)
        {
            _service = service;
        }

        #endregion Internal Constructors

        #region Public Methods

        [HttpGet]
        [Route("api/v1/counties", Name = RouteNames.County)]
        public IHttpActionResult GetAll(Guid? stateId = null, int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            var search = new ForeignKeySearch()
            {
                Id = stateId,
                Limit = limit,
                Offset = offset,
                OrderBy = orderBy
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

                // Pagination.AddPaginationHeaderToResponse(GetUrlHelper(), search, totalCount, RouteNames.County);
                var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper(), fields);

                return Ok(dynamicResult);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        #endregion Public Methods
    }

}
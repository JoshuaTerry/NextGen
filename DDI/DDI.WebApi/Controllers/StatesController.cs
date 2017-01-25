using System;
using System.Web.Http;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using Microsoft.SqlServer.Server;

namespace DDI.WebApi.Controllers
{
    public class StatesController : ControllerBase<State>
    {
        #region Private Fields

        private StateService _service;

        #endregion Private Fields

        #region Public Constructors

        public StatesController()
            : this(new StateService())
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal StatesController(StateService service)
        {
            _service = service;
        }

        #endregion Internal Constructors

        #region Public Methods

        [HttpGet]
        [Route("api/v1/states")]
        public IHttpActionResult GetAll(Guid? countryId = null, int? limit = 1000, int? offset = 0)
        {
            var search = new ForeignKeySearch()
            {
                Id = countryId,
                Limit = limit,
                Offset = offset
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
                
                // Pagination.AddPaginationHeaderToResponse(GetUrlHelper(), search, totalCount, RouteNames.State);
                var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, GetUrlHelper());

                return Ok(dynamicResult);

            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        #endregion Public Methods
    }
}

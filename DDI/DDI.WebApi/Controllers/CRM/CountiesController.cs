using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class CountiesController : GenericController<County>
    { 
        protected new CountyService Service => (CountyService)base.Service; 
   
        #region Public Methods

        [HttpGet]
        [Route("api/v1/counties", Name = RouteNames.County)]
        public IHttpActionResult GetAll(Guid? stateId = null, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
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
                var result = Service.GetAll(search);

                try
                {
                    if (result == null)
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.ToString);
                    return InternalServerError(new Exception(ex.Message));
                }

            var totalCount = result.TotalResults;
                                
                var dynamicResult = DynamicTransmogrifier.ToDynamicResponse(result, fields);
                return Ok(dynamicResult);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        #endregion Public Methods
    }

}
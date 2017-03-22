using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    public class StatesController : GenericController<State>
    {

        public StatesController()
            : base(new StateService())
        {
        }

        [HttpGet]
        [Route("api/v1/states", Name = RouteNames.State)]
        public IHttpActionResult GetAll(Guid? countryId = null, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault)
        {
                var search = new ForeignKeySearch()
                {
                    Id = countryId,
                    Limit = limit,
                    Offset = offset
                };
                return base.GetAll(RouteNames.State, search);
        }
    }
}

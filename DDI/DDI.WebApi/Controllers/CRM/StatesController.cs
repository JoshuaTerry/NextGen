using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using System;
using System.Linq.Expressions;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class StatesController : GenericController<State>
    {
        public StatesController(IService<State> service) : base(service) { }

        protected override string FieldsForList => $"{nameof(State.Id)},{nameof(State.DisplayName)},{nameof(State.StateCode)}";

        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.Cities).Exclude(p => p.Counties).Include(p => p.Country.DisplayName);

        protected override Expression<Func<State, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<State, object>>[] { p => p.Country };
        }

        [HttpGet]
        [Route("api/v1/states", Name = RouteNames.State)]
        public IHttpActionResult GetAll(Guid? countryId = null, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            if (countryId == null)
            {
                return base.GetAll(RouteNames.State, limit, offset, orderBy, fields);
            }

            return GetByCountry(countryId.Value, limit, offset, orderBy, fields);
        }


        [HttpGet]
        [Route("api/v1/states/country/{id}", Name = RouteNames.State + RouteNames.Country)]
        [Route("api/v1/countries/{id}/states", Name = RouteNames.Country + RouteNames.State)]
        public IHttpActionResult GetByCountry(Guid id, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {

            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var result = Service.GetAllWhereExpression(fp => fp.CountryId == id, search, fields);

                return FinalizeResponse(result, RouteNames.Country + RouteNames.State, search, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }

        [HttpGet]
        [Route("api/v1/states/{id}", Name = RouteNames.State + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }
    }
}

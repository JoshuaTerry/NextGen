using DDI.Services;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using DDI.Shared.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using DDI.Shared;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class CountriesController : GenericController<Country>
    {

        public CountriesController(IService<Country> service) : base(service) { }

        protected override string FieldsForList => $"{nameof(Country.Id)},{nameof(Country.DisplayName)}";

        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.States);

        [HttpGet]
        [Route("api/v1/countries", Name = RouteNames.Country)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        { 
            return base.GetAll(RouteNames.Country, limit, offset, orderBy, fields);
        }


        [HttpGet]
        [Route("api/v1/countries/{id}", Name = RouteNames.Country + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

    }
}

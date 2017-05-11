using DDI.Services;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using DDI.Shared.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class CountriesController : GenericController<Country>
    {
        public CountriesController(ServiceBase<Country> service)
            : base(service)
        {
        }
        public CountriesController()
            : base(new CountryService())
        {
        }

        protected override string FieldsForList => $"{nameof(Country.Id)},{nameof(Country.DisplayName)}";

        protected override string FieldsForSingle => new PathHelper.FieldListBuilder<Country>().IncludeAll().Exclude(p => p.States);

        protected override string FieldsForAll => FieldsForSingle;

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

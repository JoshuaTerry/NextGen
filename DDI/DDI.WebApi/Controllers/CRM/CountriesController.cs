using DDI.Shared;
using DDI.Shared.Models.Common;
using DDI.Shared.Statics;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class CountriesController : GenericController<Country>
    {

        public CountriesController(IService<Country> service) : base(service) { }

        protected override string FieldsForList => $"{nameof(Country.Id)},{nameof(Country.DisplayName)}";

        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.States);

        [HttpGet]
        [Route("api/v1/countries")]
        public override IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        { 
            return base.GetAll(limit, offset, orderBy, fields);
        }


        [HttpGet]
        [Route("api/v1/countries/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

    }
}

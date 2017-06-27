using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Enums.GL;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    public class BusinessUnitController : GenericController<BusinessUnit>
    {
        public BusinessUnitController(IService<BusinessUnit> service) : base(service) { }

        [HttpGet]
        [Route("api/v1/businessunits", Name = RouteNames.BusinessUnit)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.BusinessUnit, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/businessunits/noorganization")]
        public IHttpActionResult GetAllExceptOrganization(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            var search = PageableSearch.Max;
            fields = ConvertFieldList(fields, FieldsForList);
            var results = Service.GetAllWhereExpression(bu => bu.BusinessUnitType != BusinessUnitType.Organization, search, fields);
            return base.FinalizeResponse(results,null,search, null, null);
        }

        [HttpGet]
        [Route("api/v1/businessunits/{id}", Name = RouteNames.BusinessUnit + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/businessunits", Name = RouteNames.BusinessUnit + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] BusinessUnit entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/businessunits/{id}", Name = RouteNames.BusinessUnit + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/businessunits/{id}", Name = RouteNames.BusinessUnit + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
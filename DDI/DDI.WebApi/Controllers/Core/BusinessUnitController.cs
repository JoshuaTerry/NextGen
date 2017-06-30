using System;
using System.Web.Http;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using DDI.Services.Search;
using Newtonsoft.Json.Linq;
using DDI.Shared.Enums.GL;

namespace DDI.WebApi.Controllers.General
{
    public class BusinessUnitController : GenericController<BusinessUnit>
    {
        public BusinessUnitController(IService<BusinessUnit> service) : base(service) { }

        [HttpGet]
        [Route("api/v1/businessunits")]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/businessunits/noorganization")]
        public IHttpActionResult GetAllExceptOrganization(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            var search = PageableSearch.Max;
            fields = ConvertFieldList(fields, FieldsForList);
            var results = Service.GetAllWhereExpression(bu => bu.BusinessUnitType != BusinessUnitType.Organization, search, fields);
            return base.FinalizeResponse(results, search, null);
        }

        [HttpGet]
        [Route("api/v1/businessunits/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/businessunits")]
        public IHttpActionResult Post([FromBody] BusinessUnit entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/businessunits/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/businessunits/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
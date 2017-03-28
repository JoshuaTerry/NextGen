using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.CRM
{
    public class IncomeLevelsController : GenericController<IncomeLevel>
    {
        protected override string FieldsForList => FieldLists.CodeFields;

        [HttpGet]
        [Route("api/v1/incomelevels", Name = RouteNames.IncomeLevel)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.IncomeLevel, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/incomelevels/{id}", Name = RouteNames.IncomeLevel + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/incomelevels", Name = RouteNames.IncomeLevel + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] IncomeLevel entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/incomelevels/{id}", Name = RouteNames.IncomeLevel + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/incomelevels/{id}", Name = RouteNames.IncomeLevel + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
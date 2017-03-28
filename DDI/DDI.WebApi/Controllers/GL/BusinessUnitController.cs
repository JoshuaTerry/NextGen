using DDI.Shared.Models.Client.GL;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using System.Web.Http.Routing;

namespace DDI.WebApi.Controllers.GL
{
    public class BusinessUnitController : GenericController<BusinessUnit> 
    {



        [HttpGet]
        [Route("api/v1/businessunit")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.DoingBusinessAs, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/businessunit/{id}")]
        public new IHttpActionResult GetById(Guid id, string fields, UrlHelper urlHelper = null)
        {
            return base.GetById(id, fields, urlHelper);
        }

        [HttpPost]
        [Route("api/v1/businessunit")]
        public  IHttpActionResult Save([FromBody] BusinessUnit entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/businessunit/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/businessunit/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }

    }
}
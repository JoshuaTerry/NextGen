using System;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class MemoCodesController : ControllerBase<MemoCode>
    {
        [HttpGet]
        [Route("api/v1/MemoCodes", Name = RouteNames.MemoCode)]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.MemoCode, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/MemoCodes/{id}", Name = RouteNames.MemoCode + RouteVerbs.Get)]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/MemoCodes", Name = RouteNames.MemoCode + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] MemoCode entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/MemoCodes/{id}", Name = RouteNames.MemoCode + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/MemoCodes/{id}", Name = RouteNames.MemoCode + RouteVerbs.Delete)]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

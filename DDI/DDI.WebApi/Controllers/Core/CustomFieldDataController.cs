using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    public class CustomFieldDataController : GenericController<CustomFieldData>
    {
        public CustomFieldDataController(IService<CustomFieldData> service) : base(service) { }

        [HttpGet]
        [Route("api/v1/customfielddata")]
        public override IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/customfielddata/{id}")]
        public override IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/customfielddata")]
        public override IHttpActionResult Post([FromBody] CustomFieldData item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/customfielddata/{id}")]
        public override IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

        [HttpDelete]
        [Route("api/v1/customfielddata/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}

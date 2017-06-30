using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.IO;
using DDI.Shared;
using DDI.Services;

namespace DDI.WebApi.Controllers.Core
{
    public class SavedEntityMappingController : GenericController<SavedEntityMapping>
    {
        public SavedEntityMappingController(ServiceBase<SavedEntityMapping> service)
            : base(service)
        {
        }
        
        [HttpGet]
        [Route("api/v1/savedentitymapping")]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/savedentitymapping/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/savedentitymapping")]
        public IHttpActionResult Post([FromBody] SavedEntityMapping entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/savedentitymapping/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/savedentitymapping/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
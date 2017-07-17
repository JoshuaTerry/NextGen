using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace DDI.WebApi.Controllers.Core
{
    public class EntityMappingController : GenericController<EntityMapping>
    {
        public EntityMappingController(ServiceBase<EntityMapping> service)
            : base(service)
        {
        }
        
        [HttpGet]
        [Route("api/v1/EntityMapping")]
        public IHttpActionResult GetAll(int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/EntityMapping/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/EntityMapping/type/{type}")]
        public IHttpActionResult GetByType(Int32 type, int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = "all")
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                fields = ConvertFieldList(fields, FieldsForList);
                var result = Service.GetAllWhereExpression(s => s.MappingType.Equals(type), search, fields);

                return FinalizeResponse(result, search, fields);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }
    

        [HttpPost]
        [Route("api/v1/EntityMapping")]
        public IHttpActionResult Post([FromBody] EntityMapping entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/EntityMapping/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/EntityMapping/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
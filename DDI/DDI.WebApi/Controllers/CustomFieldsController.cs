using System;
using System.Web.Http;
using DDI.Shared.Models.Client.Core;
using DDI.Services;
using DDI.Services.Search;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;

namespace DDI.WebApi.Controllers
{
    public class CustomFieldsController : ControllerBase<CustomField>
    {
        protected new ICustomFieldService Service => (ICustomFieldService)base.Service;

        protected override Expression<Func<CustomField, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<CustomField, object>>[]
            {
                o => o.Options
            };
        }

        protected override Expression<Func<CustomField, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<CustomField, object>>[]
            {
                o => o.Options
            };
        }

        public CustomFieldsController() :base(new CustomFieldService())
        {
            
        }

        [HttpGet]
        [Route("api/v1/customfields", Name = RouteNames.CustomField)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.CustomField, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/customfields/entity/{entityId}", Name = RouteNames.CustomField + RouteNames.Entity)]
        public IHttpActionResult GetByEntity(int entityId)
        {
            try
            {
                var result = Service.GetByEntityId(entityId);

                if (result == null)
                {
                    return NotFound();
                }
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("api/v1/customfields", Name = RouteNames.CustomField + RouteVerbs.Post)]
        public IHttpActionResult Post([FromBody] CustomField item)
        {
            return base.Post(item);
        }

        [HttpPatch]
        [Route("api/v1/customfields/{id}", Name = RouteNames.CustomField + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject changes)
        {
            return base.Patch(id, changes);
        }

    }
}
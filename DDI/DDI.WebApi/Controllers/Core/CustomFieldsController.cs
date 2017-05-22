using System;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Services;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers.General
{
    [Authorize]
    public class CustomFieldsController : GenericController<CustomField>
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

        public CustomFieldsController() :base(Factory.CreateService<CustomFieldService>())
        {
            
        }

        [HttpGet]
        [Route("api/v1/customfields", Name = RouteNames.CustomField)]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(RouteNames.CustomField, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/customfields/entity/{entityId}/constituent/{constituentId}", Name = RouteNames.CustomField + RouteNames.Entity)]
        public IHttpActionResult GetByEntity(int entityId, Guid constituentId)
        {
            try
            {
                var result = Service.GetByEntityId(entityId, constituentId);

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
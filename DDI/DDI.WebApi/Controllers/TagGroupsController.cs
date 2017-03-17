using System;
using System.Linq.Expressions;
using System.Web.Http;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Enums.CRM;
using DDI.Services;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;

namespace DDI.WebApi.Controllers
{
    public class TagGroupsController : ControllerBase<TagGroup>
    {
        protected new ITagGroupService Service => (ITagGroupService)base.Service;

        protected override Expression<Func<TagGroup, object>>[] GetDataIncludesForList()
        {
            return new Expression<Func<TagGroup, object>>[]
            {
                tg => tg.Tags
            };
        }

        public TagGroupsController() : base(new TagGroupService())
        {

        }

        [HttpGet]
        [Route("api/v1/taggroups")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.Order, string fields = null)
        {
            return base.GetAll(null, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/taggroups/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/taggroups/constituentcategory/{category}")]
        public IHttpActionResult GetByConstituentCategory(ConstituentCategory category)
        {
            try
            {
                var response = Service.GetByConstituentCategory(category);

                return Ok(response);
            }
            catch(Exception ex)
            {
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPost]
        [Route("api/v1/taggroups")]
        public IHttpActionResult Post([FromBody] TagGroup entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPatch]
        [Route("api/v1/taggroups/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/taggroups/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
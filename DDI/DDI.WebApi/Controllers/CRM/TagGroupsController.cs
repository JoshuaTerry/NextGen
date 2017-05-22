using DDI.Services;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;
using System.Web.Http;
using System.Linq;
using DDI.Shared;

namespace DDI.WebApi.Controllers.CRM
{
    [Authorize]
    public class TagGroupsController : GenericController<TagGroup>
    {
        protected new ITagGroupService Service => (ITagGroupService)base.Service;

        protected override string FieldsForList => FieldLists.CodeFields;

        protected override string FieldsForAll => FieldListBuilder.IncludeAll().Exclude(p => p.Tags.First().Constituents).Exclude(p => p.Tags.First().ConstituentTypes);

        public TagGroupsController() : base(Factory.CreateService<TagGroupService>()) { }

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
        [Route("api/v1/taggroups/tags")]
        public IHttpActionResult GetGroupsAndTags(ConstituentCategory? category = null)
        {
            try
            {
                var response = Service.GetGroupsAndTags(category ?? ConstituentCategory.Both);

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
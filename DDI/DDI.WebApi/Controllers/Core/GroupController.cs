using DDI.Services;
using DDI.Services.General;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared.Models.Client.Security;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Http;

namespace DDI.WebApi.Controllers.Core
{
    [Authorize]
    public class GroupController : GenericController<Group>
    {
        protected override string FieldsForAll => FieldListBuilder
            .Include(p => p.Roles);

        protected new IGroupService Service => (IGroupService)base.Service;

        public GroupController(ServiceBase<Group> service)
            : base(service)
        {
        }

        public GroupController()
            : base(new GroupService())
        {
        }


        [HttpGet]
        [Route("api/v1/groups")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.Order, string fields = null)
        {
            return base.GetAll(null, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/groups/{groupId}/roles")]
        public IHttpActionResult GetRolesFromGroup(Guid groupId, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                // need to get the roles, not populating? Should be in the database. Also need the roles off of data
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.GetAllWhereExpression(g => g.Id == groupId);
                var results = response.Data;
                return FinalizeResponse(response, RouteNames.Note + RouteNames.NoteTopic, search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/groups/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpPost]
        [Route("api/v1/groups")]
        public IHttpActionResult Post([FromBody] Group entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPost]
        [Route("api/v1/groups/{groupId}/roles/")]
        public IHttpActionResult AddRolesToGroup(Guid groupId, [FromBody] JObject roles, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var response = Service.AddRolesToGroup(groupId, roles);
                return FinalizeResponse(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError();
            }
        }

        [HttpPatch]
        [Route("api/v1/groups/{id}")]
        public IHttpActionResult Patch(Guid id, JObject entityChanges)
        {
            return base.Patch(id, entityChanges);
        }

        [HttpDelete]
        [Route("api/v1/groups/{id}")]
        public override IHttpActionResult Delete(Guid id)
        {
            return base.Delete(id);
        }
    }
}
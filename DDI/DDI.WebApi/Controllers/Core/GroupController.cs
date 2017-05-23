using DDI.Services.Search;
using DDI.Shared.Models.Client.Security;
using DDI.Shared.Statics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DDI.WebApi.Controllers.Core
{
    [Authorize]
    public class GroupController : GenericController<Group>
    {

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
        [Route("api/v1/groups/{groupId}/roles/{roleId}")]
        public IHttpActionResult AddRoleToGroup([FromBody] Group entityToSave)
        {
            return base.Post(entityToSave);
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
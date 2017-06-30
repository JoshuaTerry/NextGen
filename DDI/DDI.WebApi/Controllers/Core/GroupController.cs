using DDI.Logger;
using DDI.Services;
using DDI.Services.Search;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
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
            .Include(p => p.DisplayName)
            .Include(p => p.Roles)
            .Include(p => p.Users);

        protected override string FieldsForSingle => FieldListBuilder
            .Include(p => p.DisplayName)
            .Include(p => p.Roles);

        protected override Expression<Func<Group, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<Group, object>>[]
            {
                n => n.Roles,
                n => n.Users
            };
        }
        public GroupController(IService<Group> service) : base(service) { }

        protected new IGroupService Service => (IGroupService)base.Service;

        [HttpGet]
        [Route("api/v1/groups")]
        public IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.Order, string fields = null)
        {
            return base.GetAll(null, limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/groups/{id}")]
        public IHttpActionResult GetById(Guid id, string fields = null)
        {
            return base.GetById(id, fields);
        }

        [HttpGet]
        [Route("api/v1/groups/{groupId}/roles")]
        public IHttpActionResult GetRolesByGroup(Guid groupId, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var group = Service.GetById(groupId);
                var roles = group.Data.Roles.ToList();

                var response = new DataResponse<List<Role>>
                {
                    Data = roles,
                    IsSuccessful = true
                };

                return FinalizeResponse<Role>(response, "", search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v1/groups/{groupId}/users")]
        public IHttpActionResult GetUsersByGroup(Guid groupId, string fields = null, int? offset = SearchParameters.OffsetDefault, int? limit = SearchParameters.LimitDefault, string orderBy = OrderByProperties.DisplayName)
        {
            try
            {
                var search = new PageableSearch(offset, limit, orderBy);
                var group = Service.GetById(groupId);
                var users = group.Data.Users.ToList();

                var response = new DataResponse<List<User>>
                {
                    Data = users,
                    IsSuccessful = true
                };

                return FinalizeResponse<User>(response, "", search, ConvertFieldList(fields, FieldsForList));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError();
            }
        }
        [HttpPost]
        [Route("api/v1/groups")]
        public IHttpActionResult Post([FromBody] Group entityToSave)
        {
            return base.Post(entityToSave);
        }

        [HttpPost]
        [Route("api/v1/groups/{groupId}/roles/")]
        public IHttpActionResult UpdateGroupRoles(Guid groupId, [FromBody] JObject roles)
        {
            try
            { 
                var response = Service.UpdateGroupRoles(groupId, roles);
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
            try
            {
                var entity = Service.GetById(id);

                if (entity.Data == null)
                {
                    return NotFound();
                }

                if (!entity.IsSuccessful)
                {
                    return BadRequest(string.Join(",", entity.ErrorMessages));
                }

                var response = Service.Delete(entity.Data);
                if (!response.IsSuccessful)
                {
                    return BadRequest(string.Join(", ", response.ErrorMessages));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }         
    }
}
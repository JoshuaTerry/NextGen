using DDI.Services.Security;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;
using DDI.Shared.Statics;
using DDI.WebApi.Models.BindingModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    public class UsersController : GenericController<User>
    {
        private UserManager _userManager;
        private RoleManager _roleManager;
        private UserService userService;


        public UsersController(UserService service) : base(service)
        {
            userService = service;
        }

        public UsersController(UserManager userManager, RoleManager roleManager) : this(Factory.CreateService<UserService>())
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public RoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().Get<RoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        protected override Expression<Func<User, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<User, object>>[]
            {
                c => c.DefaultBusinessUnit,
                c => c.BusinessUnits,
                c => c.Constituent,
                c => c.Groups
            };
        }


        [HttpGet]
        [Route("api/v1/users")]
        public override IHttpActionResult GetAll(int? limit = SearchParameters.LimitMax, int? offset = SearchParameters.OffsetDefault, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            return base.GetAll(limit, offset, orderBy, fields);
        }

        [HttpGet]
        [Route("api/v1/users/{id}")]
        public IHttpActionResult GetById(Guid id)
        {
            try
            {
                var user = Service.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/userbyname/{username}/")]
        public IHttpActionResult GetByName(string username)
        {
            try
            {
                var user = Service.GetWhereExpression(u => u.UserName == username);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/users/{id}/default/businessunit")]
        public IHttpActionResult GetDefaultBusinessUnitByUserId(Guid id)
        {
            try
            {
                var result = Service.GetById(id).Data.DefaultBusinessUnit;

                if (result == null)
                {
                    return NotFound();
                }

                var response = new DataResponse<BusinessUnit>
                {
                    Data = result,
                    IsSuccessful = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/users/{id}/businessunit")]
        [Route("api/v1/users/{id}/businessunits")]
        public IHttpActionResult GetBusinessUnitsByUserId(Guid id)
        {
            try
            {
                var result = Service.GetById(id).Data.BusinessUnits;

                if (result == null)
                {
                    return NotFound();
                }

                var response = new DataResponse<ICollection<BusinessUnit>>
                {
                    Data = result,
                    IsSuccessful = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/users/{userName}/businessunit/username")]
        public IHttpActionResult GetBusinessUnitsByUserName(string userName, int? limit = 1000, int? offset = 0, string orderBy = OrderByProperties.DisplayName, string fields = null)
        {
            try
            {
                var result = Service.GetWhereExpression(u => u.UserName == userName).Data.BusinessUnits;

                if (result == null)
                {
                    return NotFound();
                }

                var response = new DataResponse<ICollection<BusinessUnit>>
                {
                    Data = result,
                    IsSuccessful = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/users/{id}/groups")]
        public IHttpActionResult GetGroupsByUserId(Guid id)
        {
            try
            {
                var result = Service.GetById(id).Data.Groups;

                if (result == null)
                {
                    return NotFound();
                }

                var response = new DataResponse<ICollection<Group>>
                {
                    Data = result,
                    IsSuccessful = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/v1/users")]
        public async Task<IHttpActionResult> Add(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return InternalServerError(new Exception(messages.UsernameRequired));
            }

            var user = new User()
            {
                UserName = model.Username,
                Email = model.Email,
                DefaultBusinessUnitId = model.DefaultBusinessUnitId,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                ConstituentId = model.ConsitituentId
            };

            try
            {
                var result = UserManager.Create(user, model.Password);
                if (user.DefaultBusinessUnitId.HasValue)
                {
                    var buResult = AddBusinessUnitToUser(user.Id, user.DefaultBusinessUnitId.Value);
                }

                var response = new DataResponse<User>(user);
                response.IsSuccessful = true;

                return Ok(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }

        [HttpPatch]
        [Route("api/v1/users/{id}")]
        public IHttpActionResult Update(Guid id, JObject changes)
        {
            try
            {
                return base.Patch(id, changes);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPatch]
        [Route("api/v1/users/{id}/default/businessunit/{defaultbuid}")]
        public IHttpActionResult AddDefaultBusinessUnitToUser(Guid id, Guid defaultbuid)
        {
            try
            {
                var result = userService.AddDefaultBusinessUnitToUser(id, defaultbuid);

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

        [HttpPatch]
        [Route("api/v1/users/{id}/businessunit/{buid}")]
        public IHttpActionResult AddBusinessUnitToUser(Guid id, Guid buid)
        {
            try
            {
                var result = userService.AddBusinessUnitToUser(id, buid);

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

        [HttpPatch]
        [Route("api/v1/users/{id}/businessunits")]
        public IHttpActionResult UpdateUserBusinessUnits(Guid id, [FromBody] JObject businessUnits)
        {
            try
            {
                var result = userService.UpdateUserBusinessUnits(id, businessUnits);

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

        [HttpPatch]
        [Route("api/v1/users/{id}/groups")]
        public IHttpActionResult UpdateUserGroups(Guid id, [FromBody] JObject groups)
        {
            try
            {
                var result = userService.UpdateUserGroups(id, groups);

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

        [HttpDelete]
        [Route("api/v1/users/{id}")]
        public new async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await UserManager.DeleteAsync(user);

                return Ok();
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}

using DDI.Data;
using DDI.Services;
using DDI.Services.Search;
using DDI.Services.Security;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;
using DDI.Shared.Statics;
using DDI.WebApi.Helpers;
using DDI.WebApi.Models.BindingModels;
using DDI.WebApi.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Routing;

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
                c => c.Groups, 
                c => c.Constituent
            };
        }

        [Authorize(Roles = Permissions.Security_Read)]
        [HttpGet]
        [Route("api/v1/users")]
        public IHttpActionResult Get()
        {
            try {
                var results = userService.GetAll();

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
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
        [Route("api/v1/users/{userName}/businessunit")]
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


        [Authorize(Roles = Permissions.Security_ReadWrite)]
        [HttpPost]
        [Route("api/v1/users", Name = RouteNames.User + RouteVerbs.Post)]
        public IHttpActionResult Post(JObject newUser)
        {
            var user = new User() { UserName = newUser["UserName"].ToString(), Email = newUser["Email"].ToString() };
            try
            {
                var result = UserManager.Create(user);
                if (result.Succeeded)
                {
                    DataResponse<User> response  = new DataResponse<User> { Data = user, IsSuccessful = true };
                    return FinalizeResponse(response);
                }
                else
                {
                    DataResponse<User> response = new DataResponse<User> { Data = user, IsSuccessful = false };
                    return FinalizeResponse(response);
                }
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }


        [Authorize(Roles = Permissions.Security_ReadWrite)]
        [HttpPatch]
        [Route("api/v1/users/{id}", Name = RouteNames.User + RouteVerbs.Patch)]
        public IHttpActionResult Patch(Guid id, JObject userChanges)
        {
            // add user to the group
            // we only one group allowed at a time, which is why it is done outside the basic user patch
            string groupId = userChanges["GroupId"].ToString();
            if (groupId != null && groupId != "")
            {
                Guid gId = new Guid(groupId);
                IDataResponse response = userService.AddGroupToUser(id, gId);
                if (!response.IsSuccessful)
                {
                    return BadRequest(response.ErrorMessages.ToString());
                }
            }
            userChanges["GroupId"].Parent.Remove();

            List<Guid> businessUnits = new List<Guid>();
            foreach (var child in userChanges["BusinessUnitIds"].Children())
            {
                string bu = child.ToString();
                businessUnits.Add(new Guid(bu));
            }
            
            //make sure default business unit is an allowed business unit for user and add if necessary
            string defaultBusinessUnitId = userChanges["DefaultBusinessUnitId"].ToString();
            if (defaultBusinessUnitId != null && defaultBusinessUnitId != "")
            {
                Guid dbuId = new Guid(defaultBusinessUnitId);
                if (!businessUnits.Contains(dbuId))
                {
                    businessUnits.Add(dbuId);
                }
            }

            IDataResponse response2 = userService.SyncBusinessUnitsToUser(id, businessUnits);
            if (!response2.IsSuccessful)
            {
                return BadRequest(response2.ErrorMessages.ToString());
            }
            userChanges["BusinessUnitIds"].Parent.Remove();

            return base.Patch(id, userChanges);
        }




        [Authorize(Roles = Permissions.Security_ReadWrite)]
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
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}

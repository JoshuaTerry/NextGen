using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DDI.WebApi.Models;
using DDI.WebApi.Models.BindingModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using DDI.Shared.Models.Client.Core;

namespace DDI.WebApi.Controllers
{
    public class RolesController : ApiController
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public RolesController()
        {
            
        }

        public RolesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [HttpPost]
        [Route("api/v1/roles/add")]
        public async Task<IHttpActionResult> Add(RolesBindingModel model)
        {
            if (CanRolesBeAdded(model.Roles) != null)
            {
                ModelState.AddModelError("", CanRolesBeAdded(model.Roles));
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var role in model.Roles)
                {
                    await RoleManager.CreateAsync(new IdentityRole(role));
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }

        [HttpGet]
        [Route("api/v1/roles")]
        public IHttpActionResult Get()
        {
            return Ok(RoleManager.Roles.ToList());
        }

        [HttpGet]
        [Route("api/v1/roles/{roleId}/users")]
        public async Task<IHttpActionResult> GetUsersInRole(Guid roleId)
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            try
            {
                var role = await RoleManager.FindByIdAsync(roleId.ToString());
                var userIds = role.Users.ToList().Select(u => u.UserId);
                var usersToAdd = userIds.Select(id => UserManager.FindByIdAsync(Guid.Parse(id)).Result);
                users.AddRange(usersToAdd);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(users);
        }

        [HttpPatch]
        [Route("api/v1/roles/{id}/update")]
        public async Task<IHttpActionResult> Update(string id, string newRoleName)
        {
            var roleToUpdate = RoleManager.FindByIdAsync(id).Result;
            if (roleToUpdate == null)
            {
                ModelState.AddModelError("", $"Role not found.");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(newRoleName))
            {
                ModelState.AddModelError("", $"You must supply a new name for role {roleToUpdate.Name}");
                return BadRequest(ModelState);
            }

            try
            {
                roleToUpdate.Name = newRoleName;
                await RoleManager.UpdateAsync(roleToUpdate);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/v1/roles/remove")]
        public async Task<IHttpActionResult> Delete([FromBody] RolesBindingModel model)
        {
            if (CanRolesBeRemoved(model.Roles) != null)
            {
                ModelState.AddModelError("", CanRolesBeRemoved(model.Roles));
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var role in model.Roles)
                {
                    var roleToDelete = RoleManager.FindByNameAsync(role).Result;
                    if (roleToDelete != null)
                    {
                        await RoleManager.DeleteAsync(roleToDelete);
                    }
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }


        private string CanRolesBeAdded(string[] roles)
        {
            return roles.Where(role => RoleManager.RoleExists(role)).Aggregate<string, string>(null, (current, role) => current + $"Role {role} has already been created.\n");
        }

        private string CanRolesBeRemoved(string[] roles)
        {
            string errorMessage = null;

            foreach (var role in roles)
            {
                var roleToCheck = RoleManager.FindByName(role);
                if (roleToCheck != null)
                {
                    var usersInRole = roleToCheck.Users.Count;
                    if (usersInRole > 0)
                    {
                        errorMessage += $"Role {role} has {usersInRole} users associated.\n";
                    }
                }
                else
                {
                    errorMessage += $"Role {role} was not found.\n";
                }
            }

            return errorMessage;
        }
    }
}

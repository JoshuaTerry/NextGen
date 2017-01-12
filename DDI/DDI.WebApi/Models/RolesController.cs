using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WebGrease.Css.Extensions;

namespace DDI.WebApi.Models
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

        [HttpPost]
        [Route("api/v1/roles/update")]
        public async Task<IHttpActionResult> Update(string role, string newRoleName)
        {
            var roleToUpdate = RoleManager.FindByIdAsync(role).Result;
            if (roleToUpdate == null)
            {
                ModelState.AddModelError("", $"Role {roleToUpdate.Name} not found.");
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

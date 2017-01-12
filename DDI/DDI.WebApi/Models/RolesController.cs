using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace DDI.WebApi.Models
{
    public class RolesController : ApiController
    {
        private ApplicationRoleManager _roleManager;

        public RolesController()
        {
            
        }

        public RolesController(ApplicationRoleManager roleManager)
        {
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

        [HttpPost]
        [Route("api/v1/Roles/Add")]
        public async Task<IHttpActionResult> Add(string[] roles)
        {
            if (CanRolesBeAdded(roles) != null)
            {
                ModelState.AddModelError("", CanRolesBeAdded(roles));
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var role in roles)
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

        [HttpPost]
        [Route("api/v1/Roles/Add")]
        public async Task<IHttpActionResult> Remove(string[] roles)
        {
            if (CanRolesBeRemoved(roles) != null)
            {
                ModelState.AddModelError("", CanRolesBeRemoved(roles));
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var role in roles)
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
            return roles.Where(role => RoleManager.Roles.Any(r => r.Name == role)).Aggregate<string, string>(null, (current, role) => current + $"Role {role} has already been created.\n");
        }

        private string CanRolesBeRemoved(string[] roles)
        {
            return roles.Where(role => !RoleManager.Roles.Any(r => r.Name == role)).Aggregate<string, string>(null, (current, role) => current + $"Role {role} does not exist.\n");
        }
    }
}

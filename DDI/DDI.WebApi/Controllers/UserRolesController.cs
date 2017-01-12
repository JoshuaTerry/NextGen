using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using DDI.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace DDI.WebApi.Controllers
{
    public class UserRolesController : ApiController
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public UserRolesController()
        {

        }

        internal UserRolesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
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
        [Route("api/v1/UserRoles/AddRole")]
        public async Task<IHttpActionResult> AddRole(string role)
        {
            try
            {
                await RoleManager.CreateAsync(new IdentityRole(role));
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/v1/UserRoles/RemoveRole")]
        public async Task<IHttpActionResult> RemoveRole(string role)
        {
            try
            {
                var roleToDelete = RoleManager.FindByNameAsync(role).Result;
                if (roleToDelete != null)
                {
                    await RoleManager.DeleteAsync(roleToDelete);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return Ok();
        }


        [HttpGet]
        [Route("api/v1/UserRoles")]
        public async Task<IHttpActionResult> GetRolesForUser(string email)
        {
            var user = UserManager.Users.SingleOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }

            IList<string> roles;
            try
            {
                roles = await UserManager.GetRolesAsync(user.Id);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(roles);
        }

        [HttpPost]
        [Route("api/v1/UserRoles/AddSingle")]
        public async Task<IHttpActionResult> AddUserToRole([FromBody] UserRoleBindingModel model)
        {
            try
            {
                var user = UserManager.Users.SingleOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    return NotFound();
                }

                await UserManager.AddToRoleAsync(user.Id, model.Role);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/v1/UserRoles/AddMultiple")]
        public async Task<IHttpActionResult> AddUserToRoles([FromBody] UserRolesBindingModel model)
        {
            try
            {
                var user = UserManager.Users.SingleOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    return NotFound();
                }

                await UserManager.AddToRolesAsync(user.Id, model.Roles);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/v1/UserRoles/RemoveSingle")]
        public async Task<IHttpActionResult> RemoveUserFromRole([FromBody] UserRoleBindingModel model)
        {
            try
            {
                var user = UserManager.Users.SingleOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    return NotFound();
                }

                await UserManager.RemoveFromRoleAsync(user.Id, model.Role);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/v1/UserRoles/RemoveMultiple")]
        public async Task<IHttpActionResult> RemoveUserFromRoles([FromBody] UserRolesBindingModel model)
        {
            try
            {
                var user = UserManager.Users.SingleOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    return NotFound();
                }

                await UserManager.RemoveFromRolesAsync(user.Id, model.Roles);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;
using DDI.WebApi.Models;
using Microsoft.AspNet.Identity.Owin;

namespace DDI.WebApi.Controllers
{
    public class UserRolesController : ApiController
    {
        private ApplicationUserManager _userManager;

        public UserRolesController()
        {
            
        }

        internal UserRolesController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
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
        [Route("api/v1/UserRoles/AddSingle")]
        public async Task<IHttpActionResult> AddUserToRole([FromBody] UserRoleBindingModel model)
        {
            var user = UserManager.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                await UserManager.AddToRoleAsync(user.Id, model.Role);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/v1/UserRoles/AddMultiple")]
        public async Task<IHttpActionResult> AddUserToRoles([FromBody] UserRolesBindingModel model)
        {
            var user = UserManager.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
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
            var user = UserManager.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
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
            var user = UserManager.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
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

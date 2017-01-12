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

        [HttpGet]
        [Route("api/v1/userroles")]
        public async Task<IHttpActionResult> Get(string email)
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
        [Route("api/v1/userroles/add")]
        public async Task<IHttpActionResult> Add([FromBody] UserRolesBindingModel model)
        {
            if (CanRolesBeAddedToUser(model.Email, model.Roles) != null)
            {
                ModelState.AddModelError("", CanRolesBeAddedToUser(model.Email, model.Roles).Item2);
                return BadRequest(ModelState);
            }

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
        [Route("api/v1/userroles/remove")]
        public async Task<IHttpActionResult> Delete([FromBody] UserRolesBindingModel model)
        {
            if (CanRolesBeRemovedFromUser(model.Email, model.Roles) != null)
            {
                ModelState.AddModelError("", CanRolesBeRemovedFromUser(model.Email, model.Roles).Item2);
                return BadRequest(ModelState);
            }

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

        private Tuple<bool, string> CanRoleBeAddedToUser(string email, string role)
        {
            Tuple<bool, string> canRoleBeAddedToUser = null;

            var userAlreadyHasRole = UserManager.Users.SingleOrDefault(u => u.Email == email).Roles.Any(r => r.RoleId == RoleManager.FindByNameAsync(role).Result.Id);
            if (userAlreadyHasRole)
            {
                canRoleBeAddedToUser = new Tuple<bool, string>(false, $"User {email} is already in role {role}");
            }

            return canRoleBeAddedToUser;
        }

        private Tuple<bool, string> CanRolesBeAddedToUser(string email, string[] roles)
        {
            Tuple<bool, string> canRolesBeAddedToUser = null;
            string errorMessage = string.Empty;

            foreach (var role in roles)
            {
                if (CanRoleBeAddedToUser(email, role) != null)
                {
                    errorMessage += $"{CanRoleBeAddedToUser(email, role).Item2}\n";
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                canRolesBeAddedToUser = new Tuple<bool, string>(false, errorMessage);
            }

            return canRolesBeAddedToUser;
        }

        private Tuple<bool, string> CanRoleBeRemovedFromUser(string email, string role)
        {
            Tuple<bool, string> canRoleBeRemovedFromUser = null;

            var userDoesNotHaveRole = !UserManager.Users.SingleOrDefault(u => u.Email == email).Roles.Any(r => r.RoleId == RoleManager.FindByNameAsync(role).Result.Id);
            if (userDoesNotHaveRole)
            {
                canRoleBeRemovedFromUser = new Tuple<bool, string>(false, $"User {email} is not currently in role {role}");
            }

            return canRoleBeRemovedFromUser;
        }

        private Tuple<bool, string> CanRolesBeRemovedFromUser(string email, string[] roles)
        {
            Tuple<bool, string> canRolesBeRemovedFromUser = null;
            string errorMessage = string.Empty;

            foreach (var role in roles)
            {
                if (CanRoleBeRemovedFromUser(email, role) != null)
                {
                    errorMessage += $"{CanRoleBeRemovedFromUser(email, role).Item2}\n";
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                canRolesBeRemovedFromUser = new Tuple<bool, string>(false, errorMessage);
            }

            return canRolesBeRemovedFromUser;
        }
    }
}

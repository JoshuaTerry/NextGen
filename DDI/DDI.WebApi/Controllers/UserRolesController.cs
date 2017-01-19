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
using DDI.WebApi.Models.BindingModels;
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
        [Route("api/v1/users/{id}/roles")]
        public async Task<IHttpActionResult> Get(string id)
        {
            IList<string> roles;
            try
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                roles = await UserManager.GetRolesAsync(user.Id);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(roles);
        }

        [HttpPost]
        [Route("api/v1/users/roles")]
        public async Task<IHttpActionResult> Add([FromBody] UserRolesBindingModel model)
        {
            if (CanRolesBeAddedToUsers(model.Emails, model.Roles) != null)
            {
                ModelState.AddModelError("", CanRolesBeAddedToUsers(model.Emails, model.Roles).Item2);
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var email in model.Emails)
                {
                    var user = UserManager.Users.SingleOrDefault(u => u.Email == email);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    await UserManager.AddToRolesAsync(user.Id, model.Roles);

                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }


        // This endpoint is given the POST attribute as DELETE does not allow a request body and we need it in order to send arrays of users and roles
        [HttpPost]
        [Route("api/v1/users/roles/remove")]
        public async Task<IHttpActionResult> Delete([FromBody] UserRolesBindingModel model)
        {
            if (CanRolesBeRemovedFromUsers(model.Emails, model.Roles) != null)
            {
                ModelState.AddModelError("", CanRolesBeRemovedFromUsers(model.Emails, model.Roles).Item2);
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var email in model.Emails)
                {
                    var user = UserManager.Users.SingleOrDefault(u => u.Email == email);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    await UserManager.RemoveFromRolesAsync(user.Id, model.Roles);

                }
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

        private Tuple<bool, string> CanRolesBeAddedToUsers(string[] emails, string[] roles)
        {
            Tuple<bool, string> canRolesBeAddedToUser = null;
            string errorMessage = string.Empty;

            foreach (var role in roles)
            {
                foreach (var email in emails)
                {
                    if (CanRoleBeAddedToUser(email, role) != null)
                    {
                        errorMessage += $"{CanRoleBeAddedToUser(email, role).Item2}\n";
                    }
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

        private Tuple<bool, string> CanRolesBeRemovedFromUsers(string[] emails, string[] roles)
        {
            Tuple<bool, string> canRolesBeRemovedFromUser = null;
            string errorMessage = string.Empty;

            foreach (var role in roles)
            {
                foreach (var email in emails)
                {
                    if (CanRoleBeRemovedFromUser(email, role) != null)
                    {
                        errorMessage += $"{CanRoleBeRemovedFromUser(email, role).Item2}\n";
                    }
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

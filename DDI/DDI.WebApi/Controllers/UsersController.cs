using DDI.Shared.Models.Client.Core;
using DDI.WebApi.Helpers;
using DDI.WebApi.Models;
using DDI.WebApi.Models.BindingModels;
using DDI.WebApi.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace DDI.WebApi.Controllers
{
    public class UsersController : ApiController
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public UsersController()
        {
            
        }

        public UsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        [Route("api/v1/users")]
        public IHttpActionResult Get()
        {
            return Ok(UserManager.Users);
        }

        [HttpGet]
        [Route("api/v1/users/{id}")]
        public async Task<IHttpActionResult> GetById(Guid id)
        {
            ApplicationUser user;
            try
            {
                user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(user);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("api/v1/users")]
        public async Task<IHttpActionResult> Add(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
            try
            {
                var result = UserManager.Create(user, model.Password);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch(Exception ex)
            {
                string a = ex.Message;
            }
            //var result = await UserManager.CreateAsync(user, model.Password);
            

            try
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = string.Format($"http://{WebConfigurationManager.AppSettings["WEBROOT"]}/registrationConfirmation.aspx?email={new HtmlString(user.Email)}&code={code}");

                var service = new EmailService();
                var from = new MailAddress(WebConfigurationManager.AppSettings["NoReplyEmail"]);
                var to = new MailAddress(model.Email);
                var body = "Please confirm your <a href=\"" + callbackUrl + "\">email</a>.";
                var message = service.CreateMailMessage(from, to, "Confirm Your Email", body);

                service.SendMailMessage(message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok();
        }

        [HttpPatch]
        [Route("api/v1/users/{id}")]
        public async Task<IHttpActionResult> Update(Guid id, [FromBody] JObject userPropertiesChanged)
        {
            ApplicationUser user = null;
            try
            {
                user = await  UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var patchUpdateUser = new PatchUpdateUser<ApplicationUser>();
                patchUpdateUser.UpdateUser(id, userPropertiesChanged);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(UserManager.FindByIdAsync(user.Id).Result);
        }

        [HttpDelete]
        [Route("api/v1/users/{id}")]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
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

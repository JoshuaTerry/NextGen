using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using DDI.WebApi.Models;
using DDI.WebApi.Models.BindingModels;
using DDI.WebApi.Providers;
using DDI.WebApi.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using DDI.WebApi.Services;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;
using DDI.Logger;

namespace DDI.WebApi.Controllers
{
    //[Authorize]
    public class AuthorizationsController : ApiController
    {
        private const string LOCAL_LOGIN_PROVIDER = "Local";
        private UserManager _userManager;
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(AuthorizationsController));
        protected ILogger Logger => _logger;

        public AuthorizationsController()
        {
        }

        public AuthorizationsController(UserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public UserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    return Request.GetOwinContext().GetUserManager<UserManager>();
                else
                    return _userManager;
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        [Route("api/v1/Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        [HttpGet]
        [Route("api/v1/authorizations/users/{email}/roles")]
        public async Task<IHttpActionResult> GetUserRoles(string email)
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
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }

            return Ok(roles);
        }


        [Route("api/v1/authorizations/manageinfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            var user = await UserManager.FindByIdAsync(Guid.Parse(User.Identity.GetUserId())); //jlt

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (var linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LOCAL_LOGIN_PROVIDER,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LOCAL_LOGIN_PROVIDER,
                Email = user.UserName,
                Logins = logins,
            };
        }

        [Route("api/v1/authorizations/changepassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(Guid.Parse(User.Identity.GetUserId()), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [Route("api/v1/authorizations/setpassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                IdentityResult result = await UserManager.AddPasswordAsync(Guid.Parse(User.Identity.GetUserId()), model.NewPassword);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }

            return Ok();
        }


        [Route("api/v1/authorizations/removelogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            try
            {
                if (model.LoginProvider == LOCAL_LOGIN_PROVIDER)
                {
                    result = await UserManager.RemovePasswordAsync(Guid.Parse(User.Identity.GetUserId()));
                }
                else
                {
                    result = await UserManager.RemoveLoginAsync(Guid.Parse(User.Identity.GetUserId()), new UserLoginInfo(model.LoginProvider, model.ProviderKey));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }

            return Ok();
        }


        [HttpPost]
        [Route("api/v1/authorizations/confirmemail")]
        public async Task<IHttpActionResult> ConfirmEmail(ConfirmRegistrationBindingModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Code))
            {
                ModelState.AddModelError("", "User Email and Code are required");
                return BadRequest(ModelState);
            } 

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            User user;
            try
            {
                user = GetUserByEmail(model.Email);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }



            try
            {
                IdentityResult result = await UserManager.ConfirmEmailAsync(user.Id, model.Code);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/v1/authorizations/forgotpassword")]
        public async Task<IHttpActionResult> ForgotPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user;
            try
            {
                user = GetUserByEmail(email);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = string.Format($"http://{WebConfigurationManager.AppSettings["WEBROOT"]}/forgotPassword.aspx?email={new HtmlString(user.Email)}&code={code}");

            var service = new EmailService();
            var from = new MailAddress(WebConfigurationManager.AppSettings["NoReplyEmail"]);
            var to = new MailAddress(user.Email);
            var body = "Reset your <a href=\"" + callbackUrl + "\">password</a>.";
            var message = service.CreateMailMessage(from, to, "Forgotten Password", body);

            try
            {
                service.SendMailMessage(message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }
            return Ok();

        }

        [HttpPost]
        [Route("api/v1/authorizations/forgotpasswordconfirm")]
        public async Task<IHttpActionResult> ForgotPasswordConfirm(ForgotPasswordConfirmBindingModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Code) || string.IsNullOrWhiteSpace(model.NewPassword) || string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "User Email, Code, New Password, and Confirm Password are required");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user;
            try
            {
                user = GetUserByEmail(model.Email);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }

            try
            {
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.NewPassword);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString);
                return InternalServerError(new Exception(ex.Message));
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
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

        private User GetUserByEmail(string email)
        {             
            var users = UserManager.Users.Where(u => u.Email == email);
            if (users.Count() == 1)
            {
                return users.First();
            }
            else if (users.Count() > 1)
            {
                throw new Exception("Multiple users exist with this email address.");
            }
            else
            {
                throw new Exception("User not found.");
            }
        }

        #endregion
    }
}

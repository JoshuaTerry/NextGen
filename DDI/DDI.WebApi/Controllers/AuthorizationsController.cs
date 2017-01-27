﻿using System;
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

namespace DDI.WebApi.Controllers
{
    //[Authorize]
    public class AuthorizationsController : ApiController
    {
        private const string LOCAL_LOGIN_PROVIDER = "Local";
        private IApplicationUserManager _userManager;

        public AuthorizationsController()
        {
        }

        public AuthorizationsController(IApplicationUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public IApplicationUserManager UserManager
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
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok(roles);
        }


        [Route("api/v1/authorizations/manageinfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
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

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
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

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
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

            if (model.LoginProvider == LOCAL_LOGIN_PROVIDER)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
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

            ApplicationUser user;
            try
            {
                user = GetUserByEmail(model.Email);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }



            IdentityResult result = await UserManager.ConfirmEmailAsync(user.Id, model.Code);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
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

            ApplicationUser user;
            try
            {
                user = GetUserByEmail(email);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = string.Format($"http://{WebConfigurationManager.AppSettings["WEBROOT"]}/forgotPassword.aspx?email={new HtmlString(user.Email)}&code={code}");

            var service = new EmailService();
            var from = new MailAddress(WebConfigurationManager.AppSettings["NoReplyEmail"]);
            var to = new MailAddress(user.Email);
            var body = "Reset your <a href=\"" + callbackUrl + "\">password</a>.";
            var message = service.CreateMailMessage(from, to, "Forgotten Password", body);

            service.SendMailMessage(message);

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

            ApplicationUser user;
            try
            {
                user = GetUserByEmail(model.Email);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.NewPassword);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
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

        private ApplicationUser GetUserByEmail(string email)
        {
            ApplicationUser user = null;
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
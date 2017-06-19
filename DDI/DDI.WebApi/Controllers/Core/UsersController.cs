using DDI.Data;
using DDI.Services;
using DDI.Services.Search;
using DDI.Services.Security;
using DDI.Shared;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;
using DDI.WebApi.Helpers;
using DDI.WebApi.Models.BindingModels;
using DDI.WebApi.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace DDI.WebApi.Controllers.General
{
    public class UsersController : GenericController<User>
    {
        private UserManager _userManager;
        private RoleManager _roleManager;        
        private UserService userService;


        public UsersController(UserService service) : base(service)
        {
            userService = service;
        }

        public UsersController(UserManager userManager, RoleManager roleManager) : this(Factory.CreateService<UserService>())
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public UserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public RoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().Get<RoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        protected override Expression<Func<User, object>>[] GetDataIncludesForSingle()
        {
            return new Expression<Func<User, object>>[]
            {
                c => c.DefaultBusinessUnit,
                c => c.BusinessUnits,
                c => c.Constituent
            };
        }

        [HttpGet]
        [Route("api/v1/users")]
        public IHttpActionResult Get()
        {
            try {
                var results = userService.GetAll();

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/users/{id}")]
        public IHttpActionResult GetById(Guid id)
        {
            try
            {
                var user = Service.GetById(id);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/userbyname/{username}/")]
        public IHttpActionResult GetByName(string username)
        {
            try
            {
                var user = Service.GetWhereExpression(u => u.UserName == username);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/users/{id}/default/businessunit")]
        public IHttpActionResult GetDefaultBusinessUnitByUserId(Guid id)
        {
            try
            {
                var result = Service.GetById(id).Data.DefaultBusinessUnit;

                if (result == null)
                {
                    return NotFound();
                }

                var response = new DataResponse<BusinessUnit>
                {
                    Data = result,
                    IsSuccessful = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpGet]
        [Route("api/v1/users/{id}/businessunit")]
        public IHttpActionResult GetBusinessUnitsByUserId(Guid id)
        {
            try
            {
                var result = Service.GetById(id).Data.BusinessUnits;

                if (result == null)
                {
                    return NotFound();
                }

                var response = new DataResponse<ICollection<BusinessUnit>>
                {
                    Data = result,
                    IsSuccessful = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
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

            var user = new User()
            {
                UserName = model.Username,
                Email = model.Email,
                DefaultBusinessUnitId = model.DefaultBusinessUnitId,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                ConstituentId = model.ConsitituentId            
            };

            try
            {
                var result = UserManager.Create(user, model.Password);
                if (user.DefaultBusinessUnitId.HasValue)
                {
                    var buResult = AddBusinessUnitToUser(user.Id, user.DefaultBusinessUnitId.Value);
                }
            }
            catch(Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

            try
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                code = HttpUtility.UrlEncode(code);
                var callbackUrl = string.Format($"http://{WebConfigurationManager.AppSettings["WEBROOT"]}/registrationConfirmation.aspx?email={new HtmlString(user.Email)}&code={code}");

                var service = new EmailService();
                var from = new MailAddress(WebConfigurationManager.AppSettings["NoReplyEmail"]);
                var to = new MailAddress(model.Email);
                var body = "Please confirm your <a href=\"" + callbackUrl + "\">email</a>.";
                var message = service.CreateMailMessage(from, to, "Confirm Your Email", body);

                service.SendMailMessage(message);

                return Ok();
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }        
        }

        [HttpPost]
        [Route("api/v1/users/{id}")]
        public async Task<IHttpActionResult> Update(Guid id, User user)
        {             
            try
            {               
                if (user == null)
                {
                    return NotFound();
                }

                UserManager.Update(user);
                var result = await UserManager.FindByIdAsync(user.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
        }

        [HttpPatch]
        [Route("api/v1/users/{id}/default/businessunit/{defaultbuid}")]
        public IHttpActionResult AddDefaultBusinessUnitToUser(Guid id, Guid defaultbuid)
        {
            try
            {                
                var result = userService.AddDefaultBusinessUnitToUser(id, defaultbuid);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }

        [HttpPatch]
        [Route("api/v1/users/{id}/businessunit/{buid}")]
        public IHttpActionResult AddBusinessUnitToUser(Guid id, Guid buid)
        {
            try
            {                
                var result = userService.AddBusinessUnitToUser(id, buid);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }

        }

        [HttpDelete]
        [Route("api/v1/users/{id}")]
        public new async Task<IHttpActionResult> Delete(Guid id)
        {
            try
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await UserManager.DeleteAsync(user);
              
                return Ok();
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex);
                return InternalServerError(new Exception(ex.Message));
            }
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

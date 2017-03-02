using DDI.Services;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.Security;
using DDI.WebApi.Models;
using DDI.WebApi.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Threading.Tasks;

namespace DDI.WebApi
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class UserManager : UserManager<User, Guid> //, IUserManager
    { 
        public UserManager(IUserStore<User, Guid> store)
            : base(store)
        {
        }
        public override async Task<User> FindByIdAsync(Guid userId)
        {
            var user = await Store.FindByIdAsync(userId);
            return user;
        }

        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
        {
            //var manager = new UserManager(new UserStore(new ServiceBase<User>()));
            var manager = new UserManager(new UserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames 
            manager.UserValidator = new UserValidator<User, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords 
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            
            manager.RegisterTwoFactorProvider("EmailCode",  new EmailTokenProvider<User, Guid>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is: {0}"
                });
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User, Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}

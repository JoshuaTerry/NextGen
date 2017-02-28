using DDI.Shared.Models.Client.Core;
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

    public class ApplicationUserManager : UserManager<ApplicationUser, Guid> //, IApplicationUserManager
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, Guid> store)
            : base(store)
        {
        }
        public override async Task<ApplicationUser> FindByIdAsync(Guid userId)
        {
            await Task.Delay(1);
            var user = new ApplicationUser
            {
                Id = userId
            };
            var returnObject = new Task<ApplicationUser>(() => user);
            return user;
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new CustomUserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames 
            manager.UserValidator = new UserValidator<ApplicationUser, Guid>(manager)
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
            
            manager.RegisterTwoFactorProvider("EmailCode",  new EmailTokenProvider<ApplicationUser, Guid>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is: {0}"
                });
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}

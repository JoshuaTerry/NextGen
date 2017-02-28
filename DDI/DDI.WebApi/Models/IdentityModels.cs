using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DDI.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using DDI.Shared.Models;
using DDI.Shared.Models.Client.Core;

namespace DDI.WebApi.Models
{
   
    public class CustomRole : IdentityRole<Guid, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, Guid,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, Guid, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
   

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        private const string DOMAIN_CONTEXT_CONNECTION_KEY = "DomainContext";
        public ApplicationDbContext()
            : base(ConnectionManager.Instance().Connections[DOMAIN_CONTEXT_CONNECTION_KEY])            
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
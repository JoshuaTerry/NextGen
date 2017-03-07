using DDI.Shared.Models.Client.Security;
using DDI.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;

namespace DDI.WebApi
{
    public class ApplicationRoleManager : RoleManager<Role, Guid>
    {
        public ApplicationRoleManager(IRoleStore<Role, Guid> store) : base(store)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            var roleStore = new RoleStore<Role, Guid, UserRole>(context.Get<ApplicationDbContext>());
            return new ApplicationRoleManager(roleStore);
        }
    }
}
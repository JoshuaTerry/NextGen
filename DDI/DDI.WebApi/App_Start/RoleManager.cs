using DDI.Services.Security;
using DDI.Shared.Models.Client.Security;
using DDI.WebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;

namespace DDI.WebApi
{
    public class RoleManager : RoleManager<Role, Guid>
    {
        public RoleManager(IRoleStore<Role, Guid> store) : base(store)
        {
        }
        public RoleManager() : base(new RoleService()) { }

        public static RoleManager Create(IdentityFactoryOptions<RoleManager> options,
            IOwinContext context)
        {
            //var roleStore = new RoleStore<Role, Guid, UserRole>(context.Get<ApplicationDbContext>()); 
            return new RoleManager();
        }
    }
}
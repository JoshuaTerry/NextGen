using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared.Models.Client.Core
{
    public class CustomUserRole : IdentityUserRole<Guid> { }
    public class CustomUserClaim : IdentityUserClaim<Guid> { }
    public class CustomUserLogin : IdentityUserLogin<Guid> { }

    public class ApplicationUser : IdentityUser<Guid, CustomUserLogin, CustomUserRole, CustomUserClaim>, IEntity
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, Guid> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        [MaxLength(256)]
        public string FirstName { get; set; }
        [MaxLength(256)]
        public string MiddleName { get; set; }
        [MaxLength(256)]
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string DisplayName
        {
            get
            {
                return string.IsNullOrEmpty(UserName) ? $"{FirstName} {LastName}" : UserName;
            }
        }
    }
}

using DDI.Shared;
using DDI.Shared.Models.Client.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DDI.WebApi.Models
{

    public class UserStore : UserStore<User, Role, Guid, UserLogin, UserRole, UserClaim>
    {
        public UserStore(ApplicationDbContext context) : base(context)
        {
        }
    }

    public class RoleStore : RoleStore<Role, Guid, UserRole>
    {
        public RoleStore(ApplicationDbContext context) : base(context)
        {
        }
    }
    
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, UserLogin, UserRole, UserClaim>
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
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<UserLogin>().Map(c =>
            {
                c.ToTable("UserLogins");
                c.Properties(p => new
                {
                    p.Id,
                    p.UserId,
                    p.LoginProvider,
                    p.ProviderKey,
                    p.CreatedBy,
                    p.CreatedOn,
                    p.LastModifiedBy,
                    p.LastModifiedOn
                });
            }).HasKey(p => new { p.LoginProvider, p.ProviderKey, p.UserId });
            modelBuilder.Entity<UserLogin>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Mapping for ApiRole
            modelBuilder.Entity<Role>().Map(c =>
            {
                c.ToTable("Roles");
                c.Property(p => p.Id).HasColumnName("RoleId");
                c.Properties(p => new
                {
                    p.Name,
                    p.CreatedBy,
                    p.CreatedOn,
                    p.LastModifiedBy,
                    p.LastModifiedOn
                });
            }).HasKey(p => p.Id);
            modelBuilder.Entity<Role>().HasMany(c => c.Users).WithRequired().HasForeignKey(c => c.RoleId);
            modelBuilder.Entity<Role>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Role>().HasKey(p => p.Id);

            modelBuilder.Entity<User>().Map(c =>
            {
                c.ToTable("Users");
                c.Property(p => p.Id).HasColumnName("UserId");                
                c.Properties(p => new
                {
                    p.AccessFailedCount,
                    p.Email,
                    p.EmailConfirmed,
                    p.PasswordHash,
                    p.PhoneNumber,
                    p.PhoneNumberConfirmed,
                    p.TwoFactorEnabled,
                    p.SecurityStamp,
                    p.LockoutEnabled,
                    p.LockoutEndDateUtc,
                    p.UserName,
                    p.FullName,
                    p.IsActive,
                    p.LastLogin,
                    p.CreatedBy,
                    p.CreatedOn,
                    p.LastModifiedBy,
                    p.LastModifiedOn
                });
            }).HasKey(c => c.Id);
            modelBuilder.Entity<User>().HasMany(c => c.Logins).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<User>().HasMany(c => c.Claims).WithOptional().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<User>().HasMany(c => c.Roles).WithRequired().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<User>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnAnnotation("SqlDefaultValue", "newsequentialid()");            
            modelBuilder.Entity<User>().HasKey(p => p.Id);

            modelBuilder.Entity<UserRole>().Map(c =>
            {
                c.ToTable("UserRoles");
                c.Properties(p => new
                {   
                    p.Id,
                    p.UserId,
                    p.RoleId,
                    p.CreatedBy,
                    p.CreatedOn,
                    p.LastModifiedBy,
                    p.LastModifiedOn
                });
            })
            .HasKey(c => new { c.UserId, c.RoleId });
            modelBuilder.Entity<UserRole>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<UserClaim>().Map(c =>
            {
                c.ToTable("UserClaims");
                c.Property(p => p.Id).HasColumnName("UserClaimId");
                c.Properties(p => new
                {
                    p.UserId,
                    p.ClaimValue,
                    p.ClaimType,
                    p.CreatedBy,
                    p.CreatedOn,
                    p.LastModifiedBy,
                    p.LastModifiedOn
                });
            }).HasKey(c => c.Id);
            modelBuilder.Entity<UserClaim>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }
    }
}
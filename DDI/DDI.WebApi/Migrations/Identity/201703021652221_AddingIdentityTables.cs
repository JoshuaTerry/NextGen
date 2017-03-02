namespace DDI.WebApi.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIdentityTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Guid(nullable: false, identity: true),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        Id = c.Guid(nullable: false, identity: true),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Guid(nullable: false, identity: true,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newsequentialid()")
                                },
                                {
                                    "DefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newsequentialid()")
                                },
                            }),
                        FirstName = c.String(maxLength: 256),
                        MiddleName = c.String(maxLength: 256),
                        LastName = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false, defaultValue: false),
                        LastLogin = c.DateTime(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false, defaultValue: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false, defaultValue: false),
                        TwoFactorEnabled = c.Boolean(nullable: false, defaultValue: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false, defaultValue: false),
                        AccessFailedCount = c.Int(nullable: false, defaultValue: 0),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        UserClaimId = c.Guid(nullable: false, identity: true, annotations: new Dictionary<string, AnnotationValues>
                            {
                                {
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newsequentialid()")
                                },
                                {
                                    "DefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newsequentialid()")
                                },
                            }),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.UserClaimId)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Guid(nullable: false),
                        Id = c.Guid(nullable: false, identity: true, annotations: new Dictionary<string, AnnotationValues>
                            {
                                {
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newsequentialid()")
                                },
                                {
                                    "DefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newsequentialid()")
                                },
                            }), 
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserLogin", "UserId", "dbo.User");
            DropForeignKey("dbo.UserClaim", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropIndex("dbo.UserLogin", new[] { "UserId" });
            DropIndex("dbo.UserClaim", new[] { "UserId" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropTable("dbo.UserLogin");
            DropTable("dbo.UserClaim");
            DropTable("dbo.User",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "UserId",
                        new Dictionary<string, object>
                        {
                            { "SqlDefaultValue", "newsequentialid()" },
                        }
                    },
                });
            DropTable("dbo.UserRole");
            DropTable("dbo.Role");
        }
    }
}

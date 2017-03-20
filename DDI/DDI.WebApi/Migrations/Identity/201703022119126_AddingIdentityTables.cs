namespace DDI.WebApi.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class AddingIdentityTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Guid(nullable: false, identity: true,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                {
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newid()")
                                } 
                            }),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(defaultValueSql: "GETUTCDATE()"),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        Id = c.Guid(nullable: false, identity: true,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                {
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newid()")
                                } 
                            }),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(defaultValueSql: "GETUTCDATE()"),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    UserId = c.Guid(nullable: false, identity: true,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                {
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newid()")
                                }
                            }),
                    FirstName = c.String(maxLength: 256),
                    MiddleName = c.String(maxLength: 256),
                    LastName = c.String(maxLength: 256),
                    IsActive = c.Boolean(nullable: false, defaultValue: true),
                    LastLogin = c.DateTime(),
                    CreatedBy = c.Guid(),
                    CreatedOn = c.DateTime(defaultValueSql: "GETUTCDATE()"),
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
                "dbo.UserClaims",
                c => new
                    {
                        UserClaimId = c.Guid(nullable: false, identity: true,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                {
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newid()")
                                } 
                            }),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(defaultValueSql: "GETUTCDATE()"),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.UserClaimId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Guid(nullable: false),
                        Id = c.Guid(nullable: false, identity: true,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                {
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newid()")
                                },
                                {
                                    "DefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newid()")
                                }
                            }),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(defaultValueSql: "GETUTCDATE()"),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users",
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
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
        }
    }
}

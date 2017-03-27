namespace DDI.WebApi.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class UserMigrationFixes : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.BusinessUnit",
            //    c => new
            //        {
            //            Id = c.Guid(nullable: false),
            //            Name = c.String(maxLength: 128),
            //            BusinessUnitType = c.Int(nullable: false),
            //            Code = c.String(maxLength: 16),
            //            CreatedBy = c.String(maxLength: 64),
            //            CreatedOn = c.DateTime(),
            //            LastModifiedBy = c.String(maxLength: 64),
            //            LastModifiedOn = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .Index(t => t.Name, unique: true)
            //    .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.UserBusinessUnits",
                c => new
                    {
                        User_Id = c.Guid(nullable: false),
                        BusinessUnit_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.BusinessUnit_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnit_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.BusinessUnit_Id);
            
            CreateTable(
                "dbo.User1",
                c => new
                    {
                        UserId = c.Guid(nullable: false, identity: true,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "SqlDefaultValue",
                                    new AnnotationValues(oldValue: null, newValue: "newsequentialid()")
                                },
                            }),
                        DefaultBusinessUnitId = c.Guid(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.BusinessUnit", t => t.DefaultBusinessUnitId)
                .Index(t => t.DefaultBusinessUnitId);
            
            AlterColumn("dbo.Roles", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Roles", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserRoles", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserRoles", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Users", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Users", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserClaims", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserClaims", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserLogins", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.UserLogins", "LastModifiedBy", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User1", "DefaultBusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.UserBusinessUnits", "BusinessUnit_Id", "dbo.BusinessUnit");
            DropForeignKey("dbo.UserBusinessUnits", "User_Id", "dbo.Users");
            DropIndex("dbo.User1", new[] { "DefaultBusinessUnitId" });
            DropIndex("dbo.UserBusinessUnits", new[] { "BusinessUnit_Id" });
            DropIndex("dbo.UserBusinessUnits", new[] { "User_Id" });
            DropIndex("dbo.BusinessUnit", new[] { "Code" });
            DropIndex("dbo.BusinessUnit", new[] { "Name" });
            AlterColumn("dbo.UserLogins", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserLogins", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserClaims", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserClaims", "CreatedBy", c => c.String());
            AlterColumn("dbo.Users", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Users", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserRoles", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserRoles", "CreatedBy", c => c.String());
            AlterColumn("dbo.Roles", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Roles", "CreatedBy", c => c.String());
            DropTable("dbo.User1",
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
            DropTable("dbo.UserBusinessUnits");
            DropTable("dbo.BusinessUnit");
        }
    }
}

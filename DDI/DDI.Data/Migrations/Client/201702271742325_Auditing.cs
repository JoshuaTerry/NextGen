namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Auditing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangeSets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ObjectChanges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 128),
                        DisplayName = c.String(maxLength: 128),
                        ChangeType = c.String(maxLength: 64),
                        EntityId = c.String(maxLength: 128),
                        ChangeSetId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChangeSets", t => t.ChangeSetId, cascadeDelete: true)
                .Index(t => t.ChangeSetId);
            
            CreateTable(
                "dbo.PropertyChanges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChangeType = c.String(),
                        ObjectChangeId = c.Long(nullable: false),
                        PropertyName = c.String(maxLength: 128),
                        OriginalValue = c.String(maxLength: 512),
                        Value = c.String(maxLength: 512),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ObjectChanges", t => t.ObjectChangeId, cascadeDelete: true)
                .Index(t => t.ObjectChangeId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnable = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(maxLength: 256),
                        FirstName = c.String(maxLength: 256),
                        MiddleName = c.String(maxLength: 256),
                        LastName = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChangeSets", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PropertyChanges", "ObjectChangeId", "dbo.ObjectChanges");
            DropForeignKey("dbo.ObjectChanges", "ChangeSetId", "dbo.ChangeSets");
            DropIndex("dbo.PropertyChanges", new[] { "ObjectChangeId" });
            DropIndex("dbo.ObjectChanges", new[] { "ChangeSetId" });
            DropIndex("dbo.ChangeSets", new[] { "UserId" });
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.PropertyChanges");
            DropTable("dbo.ObjectChanges");
            DropTable("dbo.ChangeSets");
        }
    }
}

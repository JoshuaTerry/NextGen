namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserTables : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.DDIUsers", newName: "AspNetUsers");
            //CreateTable(
            //    "dbo.FileStorage",
            //    c => new
            //        {
            //            Id = c.Guid(nullable: false),
            //            Name = c.String(),
            //            Extension = c.String(maxLength: 256),
            //            Size = c.Long(nullable: false),
            //            Data = c.Binary(),
            //            CreatedBy = c.Guid(),
            //            CreatedOn = c.DateTime(),
            //            LastModifiedBy = c.Guid(),
            //            LastModifiedOn = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 256));
            AddColumn("dbo.AspNetUsers", "MiddleName", c => c.String(maxLength: 256));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 256));
            AddColumn("dbo.AspNetUsers", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastLogin", c => c.DateTime(nullable: true));
            AddColumn("dbo.AspNetUsers", "CreatedBy", c => c.Guid(nullable: true));
            AddColumn("dbo.AspNetUsers", "CreatedOn", c => c.DateTime(nullable: true));
            AddColumn("dbo.AspNetUsers", "LastModifiedBy", c => c.Guid(nullable: true));
            AddColumn("dbo.AspNetUsers", "LastModifiedOn", c => c.DateTime(nullable: true)); 
             
        }
        
        public override void Down()
        {
            
        }
    }
}

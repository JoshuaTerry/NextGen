namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class audit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DDIUserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.AspNetUsers", "DDIUserId");
            AddForeignKey("dbo.AspNetUsers", "DDIUserId", "dbo.DDIUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "DDIUserId", "dbo.DDIUsers");
            DropIndex("dbo.AspNetUsers", new[] { "DDIUserId" });
            DropColumn("dbo.AspNetUsers", "DDIUserId");
        }
    }
}

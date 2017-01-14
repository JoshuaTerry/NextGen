namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaritalStatus2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Constituent", "ConstituentStatusDate", c => c.DateTime());
            AddColumn("dbo.Constituent", "MaritalStatusId", c => c.Guid());
            CreateIndex("dbo.Constituent", "MaritalStatusId");
            AddForeignKey("dbo.Constituent", "MaritalStatusId", "dbo.MaritialStatus", "Id");
            DropColumn("dbo.Constituent", "MaritalStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Constituent", "MaritalStatus", c => c.Int());
            DropForeignKey("dbo.Constituent", "MaritalStatusId", "dbo.MaritialStatus");
            DropIndex("dbo.Constituent", new[] { "MaritalStatusId" });
            DropColumn("dbo.Constituent", "MaritalStatusId");
            DropColumn("dbo.Constituent", "ConstituentStatusDate");
        }
    }
}

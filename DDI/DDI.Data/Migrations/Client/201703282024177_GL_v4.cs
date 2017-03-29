namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GL_v4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FundFromTo", "Fund_Id", "dbo.Fund");
            DropIndex("dbo.FundFromTo", new[] { "Fund_Id" });
            DropColumn("dbo.FundFromTo", "Fund_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FundFromTo", "Fund_Id", c => c.Guid());
            CreateIndex("dbo.FundFromTo", "Fund_Id");
            AddForeignKey("dbo.FundFromTo", "Fund_id", "dbo.Fund");
        }
    }
}

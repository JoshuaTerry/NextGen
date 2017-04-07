namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    using Extensions;

    public partial class GL_v7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ledger", "PostDaysInAdvance", c => c.Int(nullable: false));
            AddColumn("dbo.FiscalYear", "HasAdjustmentPeriod", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ledger", "PostDaysInAdvance");
            DropColumn("dbo.FiscalYear", "HasAdjustmentPeriod");
        }
    }
}

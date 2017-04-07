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
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ledger", "PostDaysInAdvance");
        }
    }
}

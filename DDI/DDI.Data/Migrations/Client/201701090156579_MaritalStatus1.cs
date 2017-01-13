namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaritalStatus1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaritialStatus", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MaritialStatus", "IsActive");
        }
    }
}

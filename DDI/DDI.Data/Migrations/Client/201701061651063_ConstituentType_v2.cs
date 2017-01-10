namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConstituentType_v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConstituentStatus", "BaseStatus", c => c.Int(nullable: false));
            AddColumn("dbo.ConstituentStatus", "IsRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConstituentType", "Category", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConstituentType", "Category");
            DropColumn("dbo.ConstituentStatus", "IsRequired");
            DropColumn("dbo.ConstituentStatus", "BaseStatus");
        }
    }
}

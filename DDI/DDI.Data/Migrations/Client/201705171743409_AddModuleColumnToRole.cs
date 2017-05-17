namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModuleColumnToRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "Module", c => c.String(maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "Module");
        }
    }
}

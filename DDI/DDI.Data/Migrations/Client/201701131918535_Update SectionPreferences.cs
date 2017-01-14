namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSectionPreferences : DbMigration
    {
        public override void Up()
        {
           
           
            AddColumn("dbo.SectionPreference", "Value", c => c.String(maxLength: 256));
            AddColumn("dbo.SectionPreference", "IsShown", c => c.Boolean(nullable: false)); 
            DropColumn("dbo.SectionPreference", "DisplayName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SectionPreference", "DisplayName", c => c.String());
            AddColumn("dbo.ConstituentType", "BaseType", c => c.String(maxLength: 16));
            DropColumn("dbo.SectionPreference", "IsShown");
            DropColumn("dbo.SectionPreference", "Value");
            DropColumn("dbo.MaritialStatus", "IsActive");
            DropColumn("dbo.ConstituentType", "Category");
            DropColumn("dbo.ConstituentStatus", "IsRequired");
            DropColumn("dbo.ConstituentStatus", "BaseStatus");
            DropTable("dbo.Configuration");
        }
    }
}

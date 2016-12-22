namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSectionPreferenceDisplayName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SectionPreference", "DisplayName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SectionPreference", "DisplayName");
        }
    }
}

namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuditChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChangeSets", "UserName", c => c.String());
            AddColumn("dbo.PropertyChanges", "PropertyTypeName", c => c.String(maxLength: 128));
            AddColumn("dbo.PropertyChanges", "OriginalDisplayName", c => c.String(maxLength: 512));
            AddColumn("dbo.PropertyChanges", "NewValue", c => c.String(maxLength: 512));
            AddColumn("dbo.PropertyChanges", "NewDisplayName", c => c.String(maxLength: 512));
            DropColumn("dbo.PropertyChanges", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PropertyChanges", "Value", c => c.String(maxLength: 512));
            DropColumn("dbo.PropertyChanges", "NewDisplayName");
            DropColumn("dbo.PropertyChanges", "NewValue");
            DropColumn("dbo.PropertyChanges", "OriginalDisplayName");
            DropColumn("dbo.PropertyChanges", "PropertyTypeName");
            DropColumn("dbo.ChangeSets", "UserName");
        }
    }
}

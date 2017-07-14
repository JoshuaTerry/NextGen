namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CP_v2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Constituent", "Employer", c => c.String(maxLength: 128));
            AlterColumn("dbo.ContactInfo", "Info", c => c.String(maxLength: 256));
            AlterColumn("dbo.Groups", "CreatedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.Groups", "LastModifiedBy", c => c.String(maxLength: 64));
            AlterColumn("dbo.ChangeSets", "UserName", c => c.String(maxLength: 64));
            AlterColumn("dbo.PropertyChanges", "ChangeType", c => c.String(maxLength: 10));
            AlterColumn("dbo.Receipt", "Reference", c => c.String(maxLength: 128));
            AlterColumn("dbo.CustomFieldData", "Value", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CustomFieldData", "Value", c => c.String());
            AlterColumn("dbo.Receipt", "Reference", c => c.String());
            AlterColumn("dbo.PropertyChanges", "ChangeType", c => c.String());
            AlterColumn("dbo.ChangeSets", "UserName", c => c.String());
            AlterColumn("dbo.Groups", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Groups", "CreatedBy", c => c.String());
            AlterColumn("dbo.ContactInfo", "Info", c => c.String());
            AlterColumn("dbo.Constituent", "Employer", c => c.String());
        }
    }
}

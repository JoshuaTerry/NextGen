namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StringLength : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MaritialStatus", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.AddressType", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.ClergyStatus", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.ClergyType", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.ConstituentStatus", "Code", c => c.String(maxLength: 16));
            AlterColumn("dbo.ConstituentType", "BaseType", c => c.String(maxLength: 16));
            AlterColumn("dbo.ConstituentType", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.Tag", "Code", c => c.String(maxLength: 16));
            AlterColumn("dbo.ContactType", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.Denomination", "Code", c => c.String(maxLength: 16));
            AlterColumn("dbo.EducationLevel", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.Degree", "Code", c => c.String(maxLength: 16));
            AlterColumn("dbo.School", "Code", c => c.String(maxLength: 16));
            AlterColumn("dbo.Ethnicity", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.Gender", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.IncomeLevel", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.Profession", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.RelationshipType", "Code", c => c.String(maxLength: 16));
            AlterColumn("dbo.RelationshipCategory", "Code", c => c.String(maxLength: 4));
            AlterColumn("dbo.Region", "Code", c => c.String(maxLength: 16));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Region", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.RelationshipCategory", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.RelationshipType", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.Profession", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.IncomeLevel", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.Gender", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.Ethnicity", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.School", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.Degree", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.EducationLevel", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.Denomination", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.ContactType", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.Tag", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.ConstituentType", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.ConstituentType", "BaseType", c => c.String(maxLength: 128));
            AlterColumn("dbo.ConstituentStatus", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.ClergyType", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.ClergyStatus", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.AddressType", "Code", c => c.String(maxLength: 128));
            DropColumn("dbo.MaritialStatus", "Code");
        }
    }
}

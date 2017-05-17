namespace DDI.Data.Migrations.Client
{
    using System.Data.Entity.Migrations;

    public partial class ChangeCreatedOn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Address", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ConstituentAddress", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.AddressType", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Constituent", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.AlternateId", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ClergyStatus", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ClergyType", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ConstituentStatus", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ConstituentType", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Tag", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.TagGroup", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ContactInfo", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ContactType", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ContactCategory", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Denomination", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.DoingBusinessAs", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.EducationLevel", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Education", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Degree", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.School", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Ethnicity", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Gender", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.IncomeLevel", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Language", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.MaritialStatus", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.PaymentMethod", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.EFTFormat", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Prefix", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Profession", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Relationsihp", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.RelationshipType", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.RelationshipCategory", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Region", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.RegionArea", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Configuration", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.CustomField", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.CustomFieldData", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.CustomFieldOption", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.FileStorage", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.LogEntry", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.NoteCategories", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Notes", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.NoteContactMethods", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.NoteTopics", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.RegionLevel", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.SectionPreference", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SectionPreference", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.RegionLevel", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.NoteTopics", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.NoteContactMethods", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Notes", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.NoteCategories", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.LogEntry", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.FileStorage", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.CustomFieldOption", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.CustomFieldData", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.CustomField", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Configuration", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.RegionArea", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Region", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.RelationshipCategory", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.RelationshipType", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Relationsihp", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Profession", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Prefix", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.EFTFormat", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.PaymentMethod", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.MaritialStatus", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Language", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.IncomeLevel", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Gender", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Ethnicity", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.School", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Degree", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Education", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.EducationLevel", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.DoingBusinessAs", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Denomination", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ContactCategory", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ContactType", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ContactInfo", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.TagGroup", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Tag", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ConstituentType", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ConstituentStatus", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ClergyType", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ClergyStatus", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.AlternateId", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Constituent", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.AddressType", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.ConstituentAddress", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Address", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
        }
    }
}

namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertiesToConstituent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Address", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Address", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Address", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Address", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.ConstituentAddress", "CreatedBy", c => c.Guid());
            AddColumn("dbo.ConstituentAddress", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.ConstituentAddress", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.ConstituentAddress", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.AddressType", "CreatedBy", c => c.Guid());
            AddColumn("dbo.AddressType", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.AddressType", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.AddressType", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Constituent", "BirthDate", c => c.DateTime());
            AddColumn("dbo.Constituent", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Constituent", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Constituent", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Constituent", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.AlternateId", "CreatedBy", c => c.Guid());
            AddColumn("dbo.AlternateId", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.AlternateId", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.AlternateId", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.ClergyStatus", "CreatedBy", c => c.Guid());
            AddColumn("dbo.ClergyStatus", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.ClergyStatus", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.ClergyStatus", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.ClergyType", "CreatedBy", c => c.Guid());
            AddColumn("dbo.ClergyType", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.ClergyType", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.ClergyType", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.ConstituentStatus", "CreatedBy", c => c.Guid());
            AddColumn("dbo.ConstituentStatus", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.ConstituentStatus", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.ConstituentStatus", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.ConstituentType", "CreatedBy", c => c.Guid());
            AddColumn("dbo.ConstituentType", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.ConstituentType", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.ConstituentType", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Tag", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Tag", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Tag", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Tag", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.TagGroup", "CreatedBy", c => c.Guid());
            AddColumn("dbo.TagGroup", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.TagGroup", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.TagGroup", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.ContactInfo", "CreatedBy", c => c.Guid());
            AddColumn("dbo.ContactInfo", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.ContactInfo", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.ContactInfo", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.ContactType", "CreatedBy", c => c.Guid());
            AddColumn("dbo.ContactType", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.ContactType", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.ContactType", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.ContactCategory", "CreatedBy", c => c.Guid());
            AddColumn("dbo.ContactCategory", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.ContactCategory", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.ContactCategory", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Denomination", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Denomination", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Denomination", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Denomination", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.DoingBusinessAs", "CreatedBy", c => c.Guid());
            AddColumn("dbo.DoingBusinessAs", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.DoingBusinessAs", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.DoingBusinessAs", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.EducationLevel", "CreatedBy", c => c.Guid());
            AddColumn("dbo.EducationLevel", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.EducationLevel", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.EducationLevel", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Education", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Education", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Education", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Education", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Degree", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Degree", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Degree", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Degree", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.School", "CreatedBy", c => c.Guid());
            AddColumn("dbo.School", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.School", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.School", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Ethnicity", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Ethnicity", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Ethnicity", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Ethnicity", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Gender", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Gender", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Gender", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Gender", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.IncomeLevel", "CreatedBy", c => c.Guid());
            AddColumn("dbo.IncomeLevel", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.IncomeLevel", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.IncomeLevel", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Language", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Language", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Language", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Language", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.MaritialStatus", "CreatedBy", c => c.Guid());
            AddColumn("dbo.MaritialStatus", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.MaritialStatus", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.MaritialStatus", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.PaymentMethod", "CreatedBy", c => c.Guid());
            AddColumn("dbo.PaymentMethod", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.PaymentMethod", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.PaymentMethod", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.EFTFormat", "CreatedBy", c => c.Guid());
            AddColumn("dbo.EFTFormat", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.EFTFormat", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.EFTFormat", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.PaymentPreference", "CreatedBy", c => c.Guid());
            AddColumn("dbo.PaymentPreference", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.PaymentPreference", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.PaymentPreference", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Prefix", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Prefix", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Prefix", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Prefix", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Profession", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Profession", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Profession", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Profession", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Relationsihp", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Relationsihp", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Relationsihp", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Relationsihp", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.RelationshipType", "CreatedBy", c => c.Guid());
            AddColumn("dbo.RelationshipType", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.RelationshipType", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.RelationshipType", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.RelationshipCategory", "CreatedBy", c => c.Guid());
            AddColumn("dbo.RelationshipCategory", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.RelationshipCategory", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.RelationshipCategory", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Region", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Region", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Region", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Region", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.RegionArea", "CreatedBy", c => c.Guid());
            AddColumn("dbo.RegionArea", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.RegionArea", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.RegionArea", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.Configuration", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Configuration", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Configuration", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Configuration", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.CustomField", "CreatedBy", c => c.Guid());
            AddColumn("dbo.CustomField", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.CustomField", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.CustomField", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.CustomFieldData", "CreatedBy", c => c.Guid());
            AddColumn("dbo.CustomFieldData", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.CustomFieldData", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.CustomFieldData", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.CustomFieldOption", "CreatedBy", c => c.Guid());
            AddColumn("dbo.CustomFieldOption", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.CustomFieldOption", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.CustomFieldOption", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.LogEntry", "CreatedBy", c => c.Guid());
            AddColumn("dbo.LogEntry", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.LogEntry", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.LogEntry", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.RegionLevel", "CreatedBy", c => c.Guid());
            AddColumn("dbo.RegionLevel", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.RegionLevel", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.RegionLevel", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.SectionPreference", "CreatedBy", c => c.Guid());
            AddColumn("dbo.SectionPreference", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.SectionPreference", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.SectionPreference", "LastModifiedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SectionPreference", "LastModifiedOn");
            DropColumn("dbo.SectionPreference", "LastModifiedBy");
            DropColumn("dbo.SectionPreference", "CreatedOn");
            DropColumn("dbo.SectionPreference", "CreatedBy");
            DropColumn("dbo.RegionLevel", "LastModifiedOn");
            DropColumn("dbo.RegionLevel", "LastModifiedBy");
            DropColumn("dbo.RegionLevel", "CreatedOn");
            DropColumn("dbo.RegionLevel", "CreatedBy");
            DropColumn("dbo.LogEntry", "LastModifiedOn");
            DropColumn("dbo.LogEntry", "LastModifiedBy");
            DropColumn("dbo.LogEntry", "CreatedOn");
            DropColumn("dbo.LogEntry", "CreatedBy");
            DropColumn("dbo.CustomFieldOption", "LastModifiedOn");
            DropColumn("dbo.CustomFieldOption", "LastModifiedBy");
            DropColumn("dbo.CustomFieldOption", "CreatedOn");
            DropColumn("dbo.CustomFieldOption", "CreatedBy");
            DropColumn("dbo.CustomFieldData", "LastModifiedOn");
            DropColumn("dbo.CustomFieldData", "LastModifiedBy");
            DropColumn("dbo.CustomFieldData", "CreatedOn");
            DropColumn("dbo.CustomFieldData", "CreatedBy");
            DropColumn("dbo.CustomField", "LastModifiedOn");
            DropColumn("dbo.CustomField", "LastModifiedBy");
            DropColumn("dbo.CustomField", "CreatedOn");
            DropColumn("dbo.CustomField", "CreatedBy");
            DropColumn("dbo.Configuration", "LastModifiedOn");
            DropColumn("dbo.Configuration", "LastModifiedBy");
            DropColumn("dbo.Configuration", "CreatedOn");
            DropColumn("dbo.Configuration", "CreatedBy");
            DropColumn("dbo.RegionArea", "LastModifiedOn");
            DropColumn("dbo.RegionArea", "LastModifiedBy");
            DropColumn("dbo.RegionArea", "CreatedOn");
            DropColumn("dbo.RegionArea", "CreatedBy");
            DropColumn("dbo.Region", "LastModifiedOn");
            DropColumn("dbo.Region", "LastModifiedBy");
            DropColumn("dbo.Region", "CreatedOn");
            DropColumn("dbo.Region", "CreatedBy");
            DropColumn("dbo.RelationshipCategory", "LastModifiedOn");
            DropColumn("dbo.RelationshipCategory", "LastModifiedBy");
            DropColumn("dbo.RelationshipCategory", "CreatedOn");
            DropColumn("dbo.RelationshipCategory", "CreatedBy");
            DropColumn("dbo.RelationshipType", "LastModifiedOn");
            DropColumn("dbo.RelationshipType", "LastModifiedBy");
            DropColumn("dbo.RelationshipType", "CreatedOn");
            DropColumn("dbo.RelationshipType", "CreatedBy");
            DropColumn("dbo.Relationsihp", "LastModifiedOn");
            DropColumn("dbo.Relationsihp", "LastModifiedBy");
            DropColumn("dbo.Relationsihp", "CreatedOn");
            DropColumn("dbo.Relationsihp", "CreatedBy");
            DropColumn("dbo.Profession", "LastModifiedOn");
            DropColumn("dbo.Profession", "LastModifiedBy");
            DropColumn("dbo.Profession", "CreatedOn");
            DropColumn("dbo.Profession", "CreatedBy");
            DropColumn("dbo.Prefix", "LastModifiedOn");
            DropColumn("dbo.Prefix", "LastModifiedBy");
            DropColumn("dbo.Prefix", "CreatedOn");
            DropColumn("dbo.Prefix", "CreatedBy");
            DropColumn("dbo.PaymentPreference", "LastModifiedOn");
            DropColumn("dbo.PaymentPreference", "LastModifiedBy");
            DropColumn("dbo.PaymentPreference", "CreatedOn");
            DropColumn("dbo.PaymentPreference", "CreatedBy");
            DropColumn("dbo.EFTFormat", "LastModifiedOn");
            DropColumn("dbo.EFTFormat", "LastModifiedBy");
            DropColumn("dbo.EFTFormat", "CreatedOn");
            DropColumn("dbo.EFTFormat", "CreatedBy");
            DropColumn("dbo.PaymentMethod", "LastModifiedOn");
            DropColumn("dbo.PaymentMethod", "LastModifiedBy");
            DropColumn("dbo.PaymentMethod", "CreatedOn");
            DropColumn("dbo.PaymentMethod", "CreatedBy");
            DropColumn("dbo.MaritialStatus", "LastModifiedOn");
            DropColumn("dbo.MaritialStatus", "LastModifiedBy");
            DropColumn("dbo.MaritialStatus", "CreatedOn");
            DropColumn("dbo.MaritialStatus", "CreatedBy");
            DropColumn("dbo.Language", "LastModifiedOn");
            DropColumn("dbo.Language", "LastModifiedBy");
            DropColumn("dbo.Language", "CreatedOn");
            DropColumn("dbo.Language", "CreatedBy");
            DropColumn("dbo.IncomeLevel", "LastModifiedOn");
            DropColumn("dbo.IncomeLevel", "LastModifiedBy");
            DropColumn("dbo.IncomeLevel", "CreatedOn");
            DropColumn("dbo.IncomeLevel", "CreatedBy");
            DropColumn("dbo.Gender", "LastModifiedOn");
            DropColumn("dbo.Gender", "LastModifiedBy");
            DropColumn("dbo.Gender", "CreatedOn");
            DropColumn("dbo.Gender", "CreatedBy");
            DropColumn("dbo.Ethnicity", "LastModifiedOn");
            DropColumn("dbo.Ethnicity", "LastModifiedBy");
            DropColumn("dbo.Ethnicity", "CreatedOn");
            DropColumn("dbo.Ethnicity", "CreatedBy");
            DropColumn("dbo.School", "LastModifiedOn");
            DropColumn("dbo.School", "LastModifiedBy");
            DropColumn("dbo.School", "CreatedOn");
            DropColumn("dbo.School", "CreatedBy");
            DropColumn("dbo.Degree", "LastModifiedOn");
            DropColumn("dbo.Degree", "LastModifiedBy");
            DropColumn("dbo.Degree", "CreatedOn");
            DropColumn("dbo.Degree", "CreatedBy");
            DropColumn("dbo.Education", "LastModifiedOn");
            DropColumn("dbo.Education", "LastModifiedBy");
            DropColumn("dbo.Education", "CreatedOn");
            DropColumn("dbo.Education", "CreatedBy");
            DropColumn("dbo.EducationLevel", "LastModifiedOn");
            DropColumn("dbo.EducationLevel", "LastModifiedBy");
            DropColumn("dbo.EducationLevel", "CreatedOn");
            DropColumn("dbo.EducationLevel", "CreatedBy");
            DropColumn("dbo.DoingBusinessAs", "LastModifiedOn");
            DropColumn("dbo.DoingBusinessAs", "LastModifiedBy");
            DropColumn("dbo.DoingBusinessAs", "CreatedOn");
            DropColumn("dbo.DoingBusinessAs", "CreatedBy");
            DropColumn("dbo.Denomination", "LastModifiedOn");
            DropColumn("dbo.Denomination", "LastModifiedBy");
            DropColumn("dbo.Denomination", "CreatedOn");
            DropColumn("dbo.Denomination", "CreatedBy");
            DropColumn("dbo.ContactCategory", "LastModifiedOn");
            DropColumn("dbo.ContactCategory", "LastModifiedBy");
            DropColumn("dbo.ContactCategory", "CreatedOn");
            DropColumn("dbo.ContactCategory", "CreatedBy");
            DropColumn("dbo.ContactType", "LastModifiedOn");
            DropColumn("dbo.ContactType", "LastModifiedBy");
            DropColumn("dbo.ContactType", "CreatedOn");
            DropColumn("dbo.ContactType", "CreatedBy");
            DropColumn("dbo.ContactInfo", "LastModifiedOn");
            DropColumn("dbo.ContactInfo", "LastModifiedBy");
            DropColumn("dbo.ContactInfo", "CreatedOn");
            DropColumn("dbo.ContactInfo", "CreatedBy");
            DropColumn("dbo.TagGroup", "LastModifiedOn");
            DropColumn("dbo.TagGroup", "LastModifiedBy");
            DropColumn("dbo.TagGroup", "CreatedOn");
            DropColumn("dbo.TagGroup", "CreatedBy");
            DropColumn("dbo.Tag", "LastModifiedOn");
            DropColumn("dbo.Tag", "LastModifiedBy");
            DropColumn("dbo.Tag", "CreatedOn");
            DropColumn("dbo.Tag", "CreatedBy");
            DropColumn("dbo.ConstituentType", "LastModifiedOn");
            DropColumn("dbo.ConstituentType", "LastModifiedBy");
            DropColumn("dbo.ConstituentType", "CreatedOn");
            DropColumn("dbo.ConstituentType", "CreatedBy");
            DropColumn("dbo.ConstituentStatus", "LastModifiedOn");
            DropColumn("dbo.ConstituentStatus", "LastModifiedBy");
            DropColumn("dbo.ConstituentStatus", "CreatedOn");
            DropColumn("dbo.ConstituentStatus", "CreatedBy");
            DropColumn("dbo.ClergyType", "LastModifiedOn");
            DropColumn("dbo.ClergyType", "LastModifiedBy");
            DropColumn("dbo.ClergyType", "CreatedOn");
            DropColumn("dbo.ClergyType", "CreatedBy");
            DropColumn("dbo.ClergyStatus", "LastModifiedOn");
            DropColumn("dbo.ClergyStatus", "LastModifiedBy");
            DropColumn("dbo.ClergyStatus", "CreatedOn");
            DropColumn("dbo.ClergyStatus", "CreatedBy");
            DropColumn("dbo.AlternateId", "LastModifiedOn");
            DropColumn("dbo.AlternateId", "LastModifiedBy");
            DropColumn("dbo.AlternateId", "CreatedOn");
            DropColumn("dbo.AlternateId", "CreatedBy");
            DropColumn("dbo.Constituent", "LastModifiedOn");
            DropColumn("dbo.Constituent", "LastModifiedBy");
            DropColumn("dbo.Constituent", "CreatedOn");
            DropColumn("dbo.Constituent", "CreatedBy");
            DropColumn("dbo.Constituent", "BirthDate");
            DropColumn("dbo.AddressType", "LastModifiedOn");
            DropColumn("dbo.AddressType", "LastModifiedBy");
            DropColumn("dbo.AddressType", "CreatedOn");
            DropColumn("dbo.AddressType", "CreatedBy");
            DropColumn("dbo.ConstituentAddress", "LastModifiedOn");
            DropColumn("dbo.ConstituentAddress", "LastModifiedBy");
            DropColumn("dbo.ConstituentAddress", "CreatedOn");
            DropColumn("dbo.ConstituentAddress", "CreatedBy");
            DropColumn("dbo.Address", "LastModifiedOn");
            DropColumn("dbo.Address", "LastModifiedBy");
            DropColumn("dbo.Address", "CreatedOn");
            DropColumn("dbo.Address", "CreatedBy");
        }
    }
}

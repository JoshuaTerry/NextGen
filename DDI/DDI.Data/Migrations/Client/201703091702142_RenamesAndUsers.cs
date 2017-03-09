namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamesAndUsers : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Relationsihp", newName: "Relationship");
            RenameTable(name: "dbo.NoteCategories", newName: "NoteCategory");
            RenameTable(name: "dbo.Notes", newName: "Note");
            RenameTable(name: "dbo.NoteContactMethods", newName: "NoteContactMethod");
            RenameTable(name: "dbo.NoteTopics", newName: "NoteTopic");
            RenameColumn("dbo.Users", "LastName", "FullName");
            AddColumn("dbo.Note", "UserResponsibleId", c => c.Guid());
            AlterColumn("dbo.Address", "CreatedBy", c => c.String());
            AlterColumn("dbo.Address", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ConstituentAddress", "CreatedBy", c => c.String());
            AlterColumn("dbo.ConstituentAddress", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.AddressType", "CreatedBy", c => c.String());
            AlterColumn("dbo.AddressType", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Constituent", "CreatedBy", c => c.String());
            AlterColumn("dbo.Constituent", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.AlternateId", "CreatedBy", c => c.String());
            AlterColumn("dbo.AlternateId", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ClergyStatus", "CreatedBy", c => c.String());
            AlterColumn("dbo.ClergyStatus", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ClergyType", "CreatedBy", c => c.String());
            AlterColumn("dbo.ClergyType", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ConstituentStatus", "CreatedBy", c => c.String());
            AlterColumn("dbo.ConstituentStatus", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ConstituentType", "CreatedBy", c => c.String());
            AlterColumn("dbo.ConstituentType", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Tag", "CreatedBy", c => c.String());
            AlterColumn("dbo.Tag", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.TagGroup", "CreatedBy", c => c.String());
            AlterColumn("dbo.TagGroup", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ContactInfo", "CreatedBy", c => c.String());
            AlterColumn("dbo.ContactInfo", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ContactType", "CreatedBy", c => c.String());
            AlterColumn("dbo.ContactType", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ContactCategory", "CreatedBy", c => c.String());
            AlterColumn("dbo.ContactCategory", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Denomination", "CreatedBy", c => c.String());
            AlterColumn("dbo.Denomination", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.DoingBusinessAs", "CreatedBy", c => c.String());
            AlterColumn("dbo.DoingBusinessAs", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.EducationLevel", "CreatedBy", c => c.String());
            AlterColumn("dbo.EducationLevel", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Education", "CreatedBy", c => c.String());
            AlterColumn("dbo.Education", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Degree", "CreatedBy", c => c.String());
            AlterColumn("dbo.Degree", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.School", "CreatedBy", c => c.String());
            AlterColumn("dbo.School", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Ethnicity", "CreatedBy", c => c.String());
            AlterColumn("dbo.Ethnicity", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Gender", "CreatedBy", c => c.String());
            AlterColumn("dbo.Gender", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.IncomeLevel", "CreatedBy", c => c.String());
            AlterColumn("dbo.IncomeLevel", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Language", "CreatedBy", c => c.String());
            AlterColumn("dbo.Language", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.MaritialStatus", "CreatedBy", c => c.String());
            AlterColumn("dbo.MaritialStatus", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.PaymentMethod", "CreatedBy", c => c.String());
            AlterColumn("dbo.PaymentMethod", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.EFTFormat", "CreatedBy", c => c.String());
            AlterColumn("dbo.EFTFormat", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Prefix", "CreatedBy", c => c.String());
            AlterColumn("dbo.Prefix", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Profession", "CreatedBy", c => c.String());
            AlterColumn("dbo.Profession", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Relationship", "CreatedBy", c => c.String());
            AlterColumn("dbo.Relationship", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.RelationshipType", "CreatedBy", c => c.String());
            AlterColumn("dbo.RelationshipType", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.RelationshipCategory", "CreatedBy", c => c.String());
            AlterColumn("dbo.RelationshipCategory", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Region", "CreatedBy", c => c.String());
            AlterColumn("dbo.Region", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.RegionArea", "CreatedBy", c => c.String());
            AlterColumn("dbo.RegionArea", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Users", "CreatedBy", c => c.String());
            AlterColumn("dbo.Users", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Users", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserClaims", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserClaims", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserClaims", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserLogins", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserLogins", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserLogins", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserRoles", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserRoles", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserRoles", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Configuration", "CreatedBy", c => c.String());
            AlterColumn("dbo.Configuration", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.ConstituentPicture", "CreatedBy", c => c.String());
            AlterColumn("dbo.ConstituentPicture", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.FileStorage", "CreatedBy", c => c.String());
            AlterColumn("dbo.FileStorage", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.CustomField", "CreatedBy", c => c.String());
            AlterColumn("dbo.CustomField", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.CustomFieldData", "CreatedBy", c => c.String());
            AlterColumn("dbo.CustomFieldData", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.CustomFieldOption", "CreatedBy", c => c.String());
            AlterColumn("dbo.CustomFieldOption", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.LogEntry", "CreatedBy", c => c.String());
            AlterColumn("dbo.LogEntry", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.NoteCategory", "CreatedBy", c => c.String());
            AlterColumn("dbo.NoteCategory", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Note", "CreatedBy", c => c.String());
            AlterColumn("dbo.Note", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.NoteContactMethod", "CreatedBy", c => c.String());
            AlterColumn("dbo.NoteContactMethod", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.NoteTopic", "CreatedBy", c => c.String());
            AlterColumn("dbo.NoteTopic", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.NoteCode", "CreatedBy", c => c.String());
            AlterColumn("dbo.NoteCode", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.RegionLevel", "CreatedBy", c => c.String());
            AlterColumn("dbo.RegionLevel", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Roles", "CreatedBy", c => c.String());
            AlterColumn("dbo.Roles", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Roles", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.SectionPreference", "CreatedBy", c => c.String());
            AlterColumn("dbo.SectionPreference", "LastModifiedBy", c => c.String());
            CreateIndex("dbo.Note", "UserResponsibleId");
            AddForeignKey("dbo.Note", "UserResponsibleId", "dbo.Users", "UserId");
            DropColumn("dbo.Users", "FirstName");
            DropColumn("dbo.Users", "MiddleName");
            DropTable("dbo.MemoCategory");
            DropTable("dbo.MemoCode");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MemoCode",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 128),
                        Code = c.String(maxLength: 4),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MemoCategory",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 128),
                        Code = c.String(maxLength: 4),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "MiddleName", c => c.String(maxLength: 256));
            AddColumn("dbo.Users", "FirstName", c => c.String(maxLength: 256));
            DropForeignKey("dbo.Note", "UserResponsibleId", "dbo.Users");
            DropIndex("dbo.Note", new[] { "UserResponsibleId" });
            AlterColumn("dbo.SectionPreference", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.SectionPreference", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Roles", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Roles", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Roles", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.RegionLevel", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.RegionLevel", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.NoteCode", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.NoteCode", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.NoteTopic", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.NoteTopic", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.NoteContactMethod", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.NoteContactMethod", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Note", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Note", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.NoteCategory", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.NoteCategory", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.LogEntry", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.LogEntry", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.CustomFieldOption", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.CustomFieldOption", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.CustomFieldData", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.CustomFieldData", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.CustomField", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.CustomField", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.FileStorage", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.FileStorage", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.ConstituentPicture", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.ConstituentPicture", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Configuration", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Configuration", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.UserRoles", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.UserRoles", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserRoles", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.UserLogins", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.UserLogins", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserLogins", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.UserClaims", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.UserClaims", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserClaims", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Users", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Users", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Users", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.RegionArea", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.RegionArea", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Region", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Region", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.RelationshipCategory", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.RelationshipCategory", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.RelationshipType", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.RelationshipType", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Relationship", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Relationship", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Profession", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Profession", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Prefix", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Prefix", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.EFTFormat", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.EFTFormat", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.PaymentMethod", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.PaymentMethod", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.MaritialStatus", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.MaritialStatus", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Language", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Language", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.IncomeLevel", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.IncomeLevel", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Gender", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Gender", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Ethnicity", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Ethnicity", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.School", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.School", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Degree", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Degree", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Education", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Education", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.EducationLevel", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.EducationLevel", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.DoingBusinessAs", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.DoingBusinessAs", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Denomination", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Denomination", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.ContactCategory", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.ContactCategory", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.ContactType", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.ContactType", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.ContactInfo", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.ContactInfo", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.TagGroup", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.TagGroup", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Tag", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Tag", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.ConstituentType", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.ConstituentType", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.ConstituentStatus", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.ConstituentStatus", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.ClergyType", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.ClergyType", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.ClergyStatus", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.ClergyStatus", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.AlternateId", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.AlternateId", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Constituent", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Constituent", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.AddressType", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.AddressType", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.ConstituentAddress", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.ConstituentAddress", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Address", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Address", "CreatedBy", c => c.Guid());
            DropColumn("dbo.Note", "UserResponsibleId");
            RenameColumn("dbo.Users", "FullName", "LastName");
            RenameTable(name: "dbo.NoteTopic", newName: "NoteTopics");
            RenameTable(name: "dbo.NoteContactMethod", newName: "NoteContactMethods");
            RenameTable(name: "dbo.Note", newName: "Notes");
            RenameTable(name: "dbo.NoteCategory", newName: "NoteCategories");
            RenameTable(name: "dbo.Relationship", newName: "Relationsihp");
        }
    }
}

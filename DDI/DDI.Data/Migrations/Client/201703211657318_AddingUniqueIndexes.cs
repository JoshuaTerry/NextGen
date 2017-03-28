namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingUniqueIndexes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ContactType", new[] { "ContactCategoryId" });
            DropIndex("dbo.ContactCategory", new[] { "DefaultContactTypeID" });
            AlterColumn("dbo.Address", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ConstituentAddress", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.AddressType", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Constituent", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.AlternateId", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ClergyStatus", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ClergyType", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ConstituentStatus", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ConstituentType", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Tag", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.TagGroup", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ContactInfo", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ContactType", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ContactCategory", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Denomination", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.DoingBusinessAs", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.EducationLevel", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Education", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Degree", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.School", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Ethnicity", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Gender", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.IncomeLevel", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Language", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.MaritialStatus", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.PaymentMethod", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.EFTFormat", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Prefix", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Profession", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Relationship", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.RelationshipType", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.RelationshipCategory", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Region", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.RegionArea", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Configuration", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ConstituentPicture", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.FileStorage", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.CustomField", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.CustomFieldData", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.CustomFieldOption", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.NoteCategory", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Note", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.NoteContactMethod", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.NoteCode", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.NoteTopic", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.RegionLevel", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.SectionPreference", "CreatedOn", c => c.DateTime());
            CreateIndex("dbo.AddressType", "Code", unique: true);
            CreateIndex("dbo.AddressType", "Name", unique: true);
            CreateIndex("dbo.ClergyStatus", "Code", unique: true);
            CreateIndex("dbo.ClergyStatus", "Name", unique: true);
            CreateIndex("dbo.ClergyType", "Code", unique: true);
            CreateIndex("dbo.ClergyType", "Name", unique: true);
            CreateIndex("dbo.ConstituentStatus", "Code", unique: true);
            CreateIndex("dbo.ConstituentStatus", "Name", unique: true);
            CreateIndex("dbo.ConstituentType", "Code", unique: true);
            CreateIndex("dbo.ConstituentType", "Name", unique: true);
            CreateIndex("dbo.Tag", "Code", unique: true);
            CreateIndex("dbo.Tag", "Name", unique: true);
            CreateIndex("dbo.ContactType", new[] { "ContactCategoryId", "Code" }, unique: true, name: "IX_Code");
            CreateIndex("dbo.ContactType", "Name", unique: true);
            CreateIndex("dbo.ContactCategory", "Code", unique: true);
            CreateIndex("dbo.ContactCategory", "Name", unique: true);
            CreateIndex("dbo.ContactCategory", "DefaultContactTypeId");
            CreateIndex("dbo.Denomination", "Code", unique: true);
            CreateIndex("dbo.Denomination", "Name", unique: true);
            CreateIndex("dbo.EducationLevel", "Code", unique: true);
            CreateIndex("dbo.EducationLevel", "Name", unique: true);
            CreateIndex("dbo.Degree", "Code", unique: true);
            CreateIndex("dbo.Degree", "Name", unique: true);
            CreateIndex("dbo.School", "Code", unique: true);
            CreateIndex("dbo.School", "Name", unique: true);
            CreateIndex("dbo.Ethnicity", "Code", unique: true);
            CreateIndex("dbo.Ethnicity", "Name", unique: true);
            CreateIndex("dbo.Gender", "Code", unique: true);
            CreateIndex("dbo.Gender", "Name", unique: true);
            CreateIndex("dbo.IncomeLevel", "Code", unique: true);
            CreateIndex("dbo.IncomeLevel", "Name", unique: true);
            CreateIndex("dbo.Language", "Code", unique: true);
            CreateIndex("dbo.Language", "Name", unique: true);
            CreateIndex("dbo.MaritialStatus", "Code", unique: true);
            CreateIndex("dbo.MaritialStatus", "Name", unique: true);
            CreateIndex("dbo.EFTFormat", "Code", unique: true);
            CreateIndex("dbo.EFTFormat", "Name", unique: true);
            CreateIndex("dbo.Prefix", "Code", unique: true);
            CreateIndex("dbo.Prefix", "Name", unique: true);
            CreateIndex("dbo.Profession", "Name", unique: true);
            CreateIndex("dbo.Profession", "Code", unique: true);
            CreateIndex("dbo.RelationshipType", "Code", unique: true);
            CreateIndex("dbo.RelationshipType", "Name", unique: true);
            CreateIndex("dbo.Region", "Code", unique: true);
            CreateIndex("dbo.Region", "Name", unique: true);
            CreateIndex("dbo.NoteContactMethod", "Code", unique: true);
            CreateIndex("dbo.NoteContactMethod", "Name", unique: true);
            CreateIndex("dbo.NoteCode", "Code", unique: true);
            CreateIndex("dbo.NoteCode", "Name", unique: true);
            CreateIndex("dbo.NoteTopic", "Code", unique: true);
            CreateIndex("dbo.NoteTopic", "Name", unique: true);
            DropTable("dbo.LogEntry");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.LogEntry",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Exception = c.String(maxLength: 2000),
                        Level = c.String(nullable: false, maxLength: 50),
                        Logger = c.String(nullable: false, maxLength: 255),
                        Message = c.String(nullable: false, maxLength: 4000),
                        Thread = c.String(nullable: false, maxLength: 255),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.NoteTopic", new[] { "Name" });
            DropIndex("dbo.NoteTopic", new[] { "Code" });
            DropIndex("dbo.NoteCode", new[] { "Name" });
            DropIndex("dbo.NoteCode", new[] { "Code" });
            DropIndex("dbo.NoteContactMethod", new[] { "Name" });
            DropIndex("dbo.NoteContactMethod", new[] { "Code" });
            DropIndex("dbo.Region", new[] { "Name" });
            DropIndex("dbo.Region", new[] { "Code" });
            DropIndex("dbo.RelationshipType", new[] { "Name" });
            DropIndex("dbo.RelationshipType", new[] { "Code" });
            DropIndex("dbo.Profession", new[] { "Code" });
            DropIndex("dbo.Profession", new[] { "Name" });
            DropIndex("dbo.Prefix", new[] { "Name" });
            DropIndex("dbo.Prefix", new[] { "Code" });
            DropIndex("dbo.EFTFormat", new[] { "Name" });
            DropIndex("dbo.EFTFormat", new[] { "Code" });
            DropIndex("dbo.MaritialStatus", new[] { "Name" });
            DropIndex("dbo.MaritialStatus", new[] { "Code" });
            DropIndex("dbo.Language", new[] { "Name" });
            DropIndex("dbo.Language", new[] { "Code" });
            DropIndex("dbo.IncomeLevel", new[] { "Name" });
            DropIndex("dbo.IncomeLevel", new[] { "Code" });
            DropIndex("dbo.Gender", new[] { "Name" });
            DropIndex("dbo.Gender", new[] { "Code" });
            DropIndex("dbo.Ethnicity", new[] { "Name" });
            DropIndex("dbo.Ethnicity", new[] { "Code" });
            DropIndex("dbo.School", new[] { "Name" });
            DropIndex("dbo.School", new[] { "Code" });
            DropIndex("dbo.Degree", new[] { "Name" });
            DropIndex("dbo.Degree", new[] { "Code" });
            DropIndex("dbo.EducationLevel", new[] { "Name" });
            DropIndex("dbo.EducationLevel", new[] { "Code" });
            DropIndex("dbo.Denomination", new[] { "Name" });
            DropIndex("dbo.Denomination", new[] { "Code" });
            DropIndex("dbo.ContactCategory", new[] { "DefaultContactTypeId" });
            DropIndex("dbo.ContactCategory", new[] { "Name" });
            DropIndex("dbo.ContactCategory", new[] { "Code" });
            DropIndex("dbo.ContactType", new[] { "Name" });
            DropIndex("dbo.ContactType", "IX_Code");
            DropIndex("dbo.Tag", new[] { "Name" });
            DropIndex("dbo.Tag", new[] { "Code" });
            DropIndex("dbo.ConstituentType", new[] { "Name" });
            DropIndex("dbo.ConstituentType", new[] { "Code" });
            DropIndex("dbo.ConstituentStatus", new[] { "Name" });
            DropIndex("dbo.ConstituentStatus", new[] { "Code" });
            DropIndex("dbo.ClergyType", new[] { "Name" });
            DropIndex("dbo.ClergyType", new[] { "Code" });
            DropIndex("dbo.ClergyStatus", new[] { "Name" });
            DropIndex("dbo.ClergyStatus", new[] { "Code" });
            DropIndex("dbo.AddressType", new[] { "Name" });
            DropIndex("dbo.AddressType", new[] { "Code" });
            AlterColumn("dbo.SectionPreference", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.RegionLevel", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.NoteTopic", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.NoteCode", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.NoteContactMethod", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Note", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.NoteCategory", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.CustomFieldOption", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.CustomFieldData", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.CustomField", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.FileStorage", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ConstituentPicture", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Configuration", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.RegionArea", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Region", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.RelationshipCategory", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.RelationshipType", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Relationship", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Profession", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Prefix", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.EFTFormat", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.PaymentMethod", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.MaritialStatus", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Language", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.IncomeLevel", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Gender", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Ethnicity", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.School", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Degree", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Education", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.EducationLevel", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.DoingBusinessAs", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Denomination", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ContactCategory", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ContactType", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ContactInfo", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.TagGroup", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Tag", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ConstituentType", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ConstituentStatus", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ClergyType", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ClergyStatus", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.AlternateId", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Constituent", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.AddressType", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.ConstituentAddress", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Address", "CreatedOn", c => c.DateTime());
            CreateIndex("dbo.ContactCategory", "DefaultContactTypeID");
            CreateIndex("dbo.ContactType", "ContactCategoryId");
        }
    }
}

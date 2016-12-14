namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Constituent_v4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Denomination", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.DoingBusinessAs", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.Education", "DegreeId", "dbo.EducationLevel");
            DropForeignKey("dbo.EducationLevel", "EducationId", "dbo.Education");
            DropForeignKey("dbo.Ethnicity", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentAlternateId", "AlternateIdId", "dbo.AlternateId");
            DropForeignKey("dbo.ConstituentAlternateId", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentContactInfo", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentContactInfo", "ContactInfoId", "dbo.ContactInfo");
            DropForeignKey("dbo.ConstituentDBA", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentDBA", "DoingBusinessAsId", "dbo.DoingBusinessAs");
            DropForeignKey("dbo.ConstituentEducation", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentEducation", "EducationId", "dbo.Education");
            DropIndex("dbo.Denomination", new[] { "ConstituentId" });
            DropIndex("dbo.DoingBusinessAs", new[] { "Constituent_Id" });
            DropIndex("dbo.EducationLevel", new[] { "EducationId" });
            DropIndex("dbo.Education", new[] { "DegreeId" });
            DropIndex("dbo.Ethnicity", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentAlternateId", new[] { "AlternateIdId" });
            DropIndex("dbo.ConstituentAlternateId", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentContactInfo", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentContactInfo", new[] { "ContactInfoId" });
            DropIndex("dbo.ConstituentDBA", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentDBA", new[] { "DoingBusinessAsId" });
            DropIndex("dbo.ConstituentEducation", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentEducation", new[] { "EducationId" });
            CreateTable(
                "dbo.DenominationConstituents",
                c => new
                    {
                        Denomination_Id = c.Guid(nullable: false),
                        Constituent_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Denomination_Id, t.Constituent_Id })
                .ForeignKey("dbo.Denomination", t => t.Denomination_Id, cascadeDelete: true)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id, cascadeDelete: true)
                .Index(t => t.Denomination_Id)
                .Index(t => t.Constituent_Id);
            
            CreateTable(
                "dbo.DoingBusinessAsConstituents",
                c => new
                    {
                        DoingBusinessAs_Id = c.Guid(nullable: false),
                        Constituent_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.DoingBusinessAs_Id, t.Constituent_Id })
                .ForeignKey("dbo.DoingBusinessAs", t => t.DoingBusinessAs_Id, cascadeDelete: true)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id, cascadeDelete: true)
                .Index(t => t.DoingBusinessAs_Id)
                .Index(t => t.Constituent_Id);
            
            CreateTable(
                "dbo.EthnicityConstituents",
                c => new
                    {
                        Ethnicity_Id = c.Guid(nullable: false),
                        Constituent_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ethnicity_Id, t.Constituent_Id })
                .ForeignKey("dbo.Ethnicity", t => t.Ethnicity_Id, cascadeDelete: true)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id, cascadeDelete: true)
                .Index(t => t.Ethnicity_Id)
                .Index(t => t.Constituent_Id);
            
            AddColumn("dbo.AddressType", "Name", c => c.String(maxLength: 128));
            AddColumn("dbo.ClergyType", "Name", c => c.String(maxLength: 128));
            AddColumn("dbo.ConstituentType", "Name", c => c.String(maxLength: 128));
            AddColumn("dbo.Education", "StartDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.Education", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Address", "StreetAddress", c => c.String(maxLength: 255));
            AlterColumn("dbo.Address", "City", c => c.String(maxLength: 128));
            AlterColumn("dbo.Address", "PostalCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.AddressType", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "Business", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "FirstName", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "FormattedName", c => c.String(maxLength: 255));
            AlterColumn("dbo.Constituent", "LastName", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "MiddleName", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "Name", c => c.String(maxLength: 255));
            AlterColumn("dbo.Constituent", "Name2", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "NameFormat", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "Nickname", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "PlaceOfOrdination", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "Position", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "Salutation", c => c.String(maxLength: 255));
            AlterColumn("dbo.Constituent", "Source", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "Suffix", c => c.String(maxLength: 128));
            AlterColumn("dbo.Constituent", "TaxId", c => c.String(maxLength: 128));
            AlterColumn("dbo.AlternateId", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.ClergyStatus", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.ClergyStatus", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.ClergyType", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.ConstituentStatus", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.ConstituentStatus", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.ConstituentType", "BaseType", c => c.String(maxLength: 128));
            AlterColumn("dbo.ConstituentType", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.ContactType", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.ContactType", "Description", c => c.String(maxLength: 128));
            AlterColumn("dbo.Denomination", "Affiliation", c => c.String(maxLength: 128));
            AlterColumn("dbo.Denomination", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.Denomination", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.Denomination", "Religion", c => c.String(maxLength: 128));
            AlterColumn("dbo.DoingBusinessAs", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.DoingBusinessAs", "StartDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.DoingBusinessAs", "EndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.EducationLevel", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.EducationLevel", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.Education", "Major", c => c.String(maxLength: 128));
            AlterColumn("dbo.Education", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.Education", "School", c => c.String(maxLength: 128));
            AlterColumn("dbo.Education", "SchoolCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.Education", "SchoolOther", c => c.String(maxLength: 128));
            AlterColumn("dbo.Education", "DegreeCode", c => c.String(maxLength: 128));
            AlterColumn("dbo.Education", "DegreeOther", c => c.String(maxLength: 128));
            AlterColumn("dbo.Ethnicity", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.Ethnicity", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.Gender", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.Gender", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.IncomeLevel", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.IncomeLevel", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.Language", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.Language", "Code", c => c.String(maxLength: 128));
            AlterColumn("dbo.PaymentPreference", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.PaymentPreference", "AccountNumber", c => c.String(maxLength: 128));
            AlterColumn("dbo.PaymentPreference", "AccountType", c => c.String(maxLength: 128));
            AlterColumn("dbo.Prefix", "Abbreviation", c => c.String(maxLength: 128));
            AlterColumn("dbo.Prefix", "Description", c => c.String(maxLength: 128));
            AlterColumn("dbo.Profession", "Name", c => c.String(maxLength: 128));
            AlterColumn("dbo.Profession", "Code", c => c.String(maxLength: 128));
            DropColumn("dbo.AddressType", "Description");
            DropColumn("dbo.ClergyStatus", "Description");
            DropColumn("dbo.ClergyType", "Description");
            DropColumn("dbo.ConstituentType", "Description");
            DropColumn("dbo.Denomination", "ConstituentId");
            DropColumn("dbo.DoingBusinessAs", "Constituent_Id");
            DropColumn("dbo.EducationLevel", "EducationId");
            DropColumn("dbo.Education", "Start");
            DropColumn("dbo.Education", "DegreeId");
            DropColumn("dbo.Education", "End");
            DropColumn("dbo.Ethnicity", "ConstituentId");
            DropTable("dbo.ConstituentAlternateId");
            DropTable("dbo.ConstituentContactInfo");
            DropTable("dbo.ConstituentDBA");
            DropTable("dbo.ConstituentEducation");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ConstituentEducation",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ConstituentId = c.Guid(),
                        EducationId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstituentDBA",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ConstituentId = c.Guid(),
                        DoingBusinessAsId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstituentContactInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ConstituentId = c.Guid(),
                        ContactInfoId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstituentAlternateId",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AlternateIdId = c.Guid(),
                        ConstituentId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Ethnicity", "ConstituentId", c => c.Guid());
            AddColumn("dbo.Education", "End", c => c.DateTime());
            AddColumn("dbo.Education", "DegreeId", c => c.Guid());
            AddColumn("dbo.Education", "Start", c => c.DateTime());
            AddColumn("dbo.EducationLevel", "EducationId", c => c.Guid());
            AddColumn("dbo.DoingBusinessAs", "Constituent_Id", c => c.Guid());
            AddColumn("dbo.Denomination", "ConstituentId", c => c.Guid());
            AddColumn("dbo.ConstituentType", "Description", c => c.String());
            AddColumn("dbo.ClergyType", "Description", c => c.String());
            AddColumn("dbo.ClergyStatus", "Description", c => c.String());
            AddColumn("dbo.AddressType", "Description", c => c.String());
            DropForeignKey("dbo.EthnicityConstituents", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.EthnicityConstituents", "Ethnicity_Id", "dbo.Ethnicity");
            DropForeignKey("dbo.DoingBusinessAsConstituents", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.DoingBusinessAsConstituents", "DoingBusinessAs_Id", "dbo.DoingBusinessAs");
            DropForeignKey("dbo.DenominationConstituents", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.DenominationConstituents", "Denomination_Id", "dbo.Denomination");
            DropIndex("dbo.EthnicityConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.EthnicityConstituents", new[] { "Ethnicity_Id" });
            DropIndex("dbo.DoingBusinessAsConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.DoingBusinessAsConstituents", new[] { "DoingBusinessAs_Id" });
            DropIndex("dbo.DenominationConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.DenominationConstituents", new[] { "Denomination_Id" });
            AlterColumn("dbo.Profession", "Code", c => c.String());
            AlterColumn("dbo.Profession", "Name", c => c.String());
            AlterColumn("dbo.Prefix", "Description", c => c.String());
            AlterColumn("dbo.Prefix", "Abbreviation", c => c.String());
            AlterColumn("dbo.PaymentPreference", "AccountType", c => c.String());
            AlterColumn("dbo.PaymentPreference", "AccountNumber", c => c.String());
            AlterColumn("dbo.PaymentPreference", "Name", c => c.String());
            AlterColumn("dbo.Language", "Code", c => c.String());
            AlterColumn("dbo.Language", "Name", c => c.String());
            AlterColumn("dbo.IncomeLevel", "Code", c => c.String());
            AlterColumn("dbo.IncomeLevel", "Name", c => c.String());
            AlterColumn("dbo.Gender", "Code", c => c.String());
            AlterColumn("dbo.Gender", "Name", c => c.String());
            AlterColumn("dbo.Ethnicity", "Code", c => c.String());
            AlterColumn("dbo.Ethnicity", "Name", c => c.String());
            AlterColumn("dbo.Education", "DegreeOther", c => c.String());
            AlterColumn("dbo.Education", "DegreeCode", c => c.String());
            AlterColumn("dbo.Education", "SchoolOther", c => c.String());
            AlterColumn("dbo.Education", "SchoolCode", c => c.String());
            AlterColumn("dbo.Education", "School", c => c.String());
            AlterColumn("dbo.Education", "Name", c => c.String());
            AlterColumn("dbo.Education", "Major", c => c.String());
            AlterColumn("dbo.EducationLevel", "Code", c => c.String());
            AlterColumn("dbo.EducationLevel", "Name", c => c.String());
            AlterColumn("dbo.DoingBusinessAs", "EndDate", c => c.DateTime());
            AlterColumn("dbo.DoingBusinessAs", "StartDate", c => c.DateTime());
            AlterColumn("dbo.DoingBusinessAs", "Name", c => c.String());
            AlterColumn("dbo.Denomination", "Religion", c => c.String());
            AlterColumn("dbo.Denomination", "Name", c => c.String());
            AlterColumn("dbo.Denomination", "Code", c => c.String());
            AlterColumn("dbo.Denomination", "Affiliation", c => c.String());
            AlterColumn("dbo.ContactType", "Description", c => c.String());
            AlterColumn("dbo.ContactType", "Code", c => c.String());
            AlterColumn("dbo.ConstituentType", "Code", c => c.String());
            AlterColumn("dbo.ConstituentType", "BaseType", c => c.String());
            AlterColumn("dbo.ConstituentStatus", "Name", c => c.String());
            AlterColumn("dbo.ConstituentStatus", "Code", c => c.String());
            AlterColumn("dbo.ClergyType", "Code", c => c.String());
            AlterColumn("dbo.ClergyStatus", "Name", c => c.String());
            AlterColumn("dbo.ClergyStatus", "Code", c => c.String());
            AlterColumn("dbo.AlternateId", "Name", c => c.String());
            AlterColumn("dbo.Constituent", "TaxId", c => c.String());
            AlterColumn("dbo.Constituent", "Suffix", c => c.String());
            AlterColumn("dbo.Constituent", "Source", c => c.String());
            AlterColumn("dbo.Constituent", "Salutation", c => c.String());
            AlterColumn("dbo.Constituent", "Position", c => c.String());
            AlterColumn("dbo.Constituent", "PlaceOfOrdination", c => c.String());
            AlterColumn("dbo.Constituent", "Nickname", c => c.String());
            AlterColumn("dbo.Constituent", "NameFormat", c => c.String());
            AlterColumn("dbo.Constituent", "Name2", c => c.String());
            AlterColumn("dbo.Constituent", "Name", c => c.String());
            AlterColumn("dbo.Constituent", "MiddleName", c => c.String());
            AlterColumn("dbo.Constituent", "LastName", c => c.String());
            AlterColumn("dbo.Constituent", "FormattedName", c => c.String());
            AlterColumn("dbo.Constituent", "FirstName", c => c.String());
            AlterColumn("dbo.Constituent", "Business", c => c.String());
            AlterColumn("dbo.AddressType", "Code", c => c.String());
            AlterColumn("dbo.Address", "PostalCode", c => c.String());
            AlterColumn("dbo.Address", "City", c => c.String());
            AlterColumn("dbo.Address", "StreetAddress", c => c.String());
            DropColumn("dbo.Education", "EndDate");
            DropColumn("dbo.Education", "StartDate");
            DropColumn("dbo.ConstituentType", "Name");
            DropColumn("dbo.ClergyType", "Name");
            DropColumn("dbo.AddressType", "Name");
            DropTable("dbo.EthnicityConstituents");
            DropTable("dbo.DoingBusinessAsConstituents");
            DropTable("dbo.DenominationConstituents");
            CreateIndex("dbo.ConstituentEducation", "EducationId");
            CreateIndex("dbo.ConstituentEducation", "ConstituentId");
            CreateIndex("dbo.ConstituentDBA", "DoingBusinessAsId");
            CreateIndex("dbo.ConstituentDBA", "ConstituentId");
            CreateIndex("dbo.ConstituentContactInfo", "ContactInfoId");
            CreateIndex("dbo.ConstituentContactInfo", "ConstituentId");
            CreateIndex("dbo.ConstituentAlternateId", "ConstituentId");
            CreateIndex("dbo.ConstituentAlternateId", "AlternateIdId");
            CreateIndex("dbo.Ethnicity", "ConstituentId");
            CreateIndex("dbo.Education", "DegreeId");
            CreateIndex("dbo.EducationLevel", "EducationId");
            CreateIndex("dbo.DoingBusinessAs", "Constituent_Id");
            CreateIndex("dbo.Denomination", "ConstituentId");
            AddForeignKey("dbo.ConstituentEducation", "EducationId", "dbo.Education", "Id");
            AddForeignKey("dbo.ConstituentEducation", "ConstituentId", "dbo.Constituent", "Id");
            AddForeignKey("dbo.ConstituentDBA", "DoingBusinessAsId", "dbo.DoingBusinessAs", "Id");
            AddForeignKey("dbo.ConstituentDBA", "ConstituentId", "dbo.Constituent", "Id");
            AddForeignKey("dbo.ConstituentContactInfo", "ContactInfoId", "dbo.ContactInfo", "Id");
            AddForeignKey("dbo.ConstituentContactInfo", "ConstituentId", "dbo.Constituent", "Id");
            AddForeignKey("dbo.ConstituentAlternateId", "ConstituentId", "dbo.Constituent", "Id");
            AddForeignKey("dbo.ConstituentAlternateId", "AlternateIdId", "dbo.AlternateId", "Id");
            AddForeignKey("dbo.Ethnicity", "ConstituentId", "dbo.Constituent", "Id");
            AddForeignKey("dbo.EducationLevel", "EducationId", "dbo.Education", "Id");
            AddForeignKey("dbo.Education", "DegreeId", "dbo.EducationLevel", "Id");
            AddForeignKey("dbo.DoingBusinessAs", "Constituent_Id", "dbo.Constituent", "Id");
            AddForeignKey("dbo.Denomination", "ConstituentId", "dbo.Constituent", "Id");
        }
    }
}

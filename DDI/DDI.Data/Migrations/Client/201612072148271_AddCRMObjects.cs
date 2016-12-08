namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCRMObjects : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        City = c.String(),
                        Line1 = c.String(),
                        Line2 = c.String(),
                        StateId = c.Guid(),
                        Zip = c.String(),
                        Constituent_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.State", t => t.StateId)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id)
                .Index(t => t.StateId)
                .Index(t => t.Constituent_Id);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Abbreviation = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AlternateId",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ConstituentId = c.Guid(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .Index(t => t.ConstituentId);
            
            CreateTable(
                "dbo.Constituent",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        BirthDate = c.DateTime(),
                        BirthYearFrom = c.Int(),
                        BirthYearTo = c.Int(),
                        Business = c.String(),
                        ClergyStatusId = c.Guid(),
                        ClergyTypeId = c.Guid(),
                        ConstituentNum = c.Int(nullable: false),
                        ConstituentStatusId = c.Guid(),
                        ConstituentTypeId = c.Guid(),
                        DeceasedDate = c.DateTime(),
                        DivorceDate = c.DateTime(),
                        Employer = c.String(),
                        EmploymentEndDate = c.DateTime(),
                        EmploymentStartDate = c.DateTime(),
                        FirstEmploymentDate = c.DateTime(),
                        FirstName = c.String(),
                        FormattedName = c.String(),
                        GenderId = c.Guid(),
                        IncomeLevelId = c.Guid(),
                        IsEmployee = c.Boolean(nullable: false),
                        IsIRSLetterReceived = c.Boolean(nullable: false),
                        IsTaxExempt = c.Boolean(nullable: false),
                        LanguageId = c.Guid(),
                        LastName = c.String(),
                        MaritalStatus = c.Int(),
                        MarriageDate = c.DateTime(),
                        MembershipCount = c.Int(),
                        MiddleName = c.String(),
                        Name2 = c.String(),
                        Nickname = c.String(),
                        OrdinationDate = c.DateTime(storeType: "date"),
                        PlaceOfOrdination = c.String(),
                        Position = c.String(),
                        PrefixId = c.Guid(),
                        ProfessionId = c.Guid(),
                        ProspectDate = c.DateTime(),
                        Salutation = c.String(),
                        Source = c.String(),
                        Suffix = c.String(),
                        TaxExemptVerifyDate = c.DateTime(),
                        TaxId = c.String(),
                        YearEstablished = c.Int(),
                        EducationLevel_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClergyStatus", t => t.ClergyStatusId)
                .ForeignKey("dbo.ClergyType", t => t.ClergyTypeId)
                .ForeignKey("dbo.ConstituentStatus", t => t.ConstituentStatusId)
                .ForeignKey("dbo.ConstituentType", t => t.ConstituentTypeId)
                .ForeignKey("dbo.EducationLevel", t => t.EducationLevel_Id)
                .ForeignKey("dbo.Gender", t => t.GenderId)
                .ForeignKey("dbo.IncomeLevel", t => t.IncomeLevelId)
                .ForeignKey("dbo.Language", t => t.LanguageId)
                .ForeignKey("dbo.Prefix", t => t.PrefixId)
                .ForeignKey("dbo.Profession", t => t.ProfessionId)
                .Index(t => t.ClergyStatusId)
                .Index(t => t.ClergyTypeId)
                .Index(t => t.ConstituentStatusId)
                .Index(t => t.ConstituentTypeId)
                .Index(t => t.GenderId)
                .Index(t => t.IncomeLevelId)
                .Index(t => t.LanguageId)
                .Index(t => t.PrefixId)
                .Index(t => t.ProfessionId)
                .Index(t => t.EducationLevel_Id);
            
            CreateTable(
                "dbo.ClergyStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClergyType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstituentStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstituentType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        BaseType = c.String(),
                        Code = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Comment = c.String(),
                        ConstituentId = c.Guid(),
                        ContactTypeId = c.Guid(),
                        Info = c.String(),
                        IsPreferred = c.Boolean(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.ContactType", t => t.ContactTypeId)
                .Index(t => t.ConstituentId)
                .Index(t => t.ContactTypeId);
            
            CreateTable(
                "dbo.ContactType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Denomination",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Affiliation = c.String(),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(),
                        Religion = c.String(),
                        Constituent_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id)
                .Index(t => t.Constituent_Id);
            
            CreateTable(
                "dbo.DoingBusinessAs",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        EndDate = c.DateTime(),
                        Name = c.String(),
                        StartDate = c.DateTime(),
                        Constituent_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id)
                .Index(t => t.Constituent_Id);
            
            CreateTable(
                "dbo.EducationLevel",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Education",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        DegreeId = c.Guid(),
                        End = c.DateTime(),
                        Major = c.String(),
                        Name = c.String(),
                        School = c.String(),
                        Start = c.DateTime(),
                        Constituent_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EducationLevel", t => t.DegreeId)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id)
                .Index(t => t.DegreeId)
                .Index(t => t.Constituent_Id);
            
            CreateTable(
                "dbo.Ethnicity",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(),
                        Constituent_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id)
                .Index(t => t.Constituent_Id);
            
            CreateTable(
                "dbo.Gender",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(),
                        IsMasculine = c.Boolean(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncomeLevel",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentPreference",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ABANumber = c.Int(nullable: false),
                        AccountNumber = c.String(),
                        AccountType = c.String(),
                        Name = c.String(),
                        Constituent_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id)
                .Index(t => t.Constituent_Id);
            
            CreateTable(
                "dbo.Prefix",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Abbreviation = c.String(),
                        Descriptin = c.String(),
                        GenderId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gender", t => t.GenderId)
                .Index(t => t.GenderId);
            
            CreateTable(
                "dbo.Profession",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstituentAddress",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AddressId = c.Guid(),
                        ConstituentId = c.Guid(),
                        IsPrimary = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.AddressId)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .Index(t => t.AddressId)
                .Index(t => t.ConstituentId);
            
            CreateTable(
                "dbo.ConstituentAlternateId",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AlternateIdId = c.Guid(),
                        ConstituentId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AlternateId", t => t.AlternateIdId)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .Index(t => t.AlternateIdId)
                .Index(t => t.ConstituentId);
            
            CreateTable(
                "dbo.ConstituentContactInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ConstituentId = c.Guid(),
                        ContactInfoId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.ContactInfo", t => t.ContactInfoId)
                .Index(t => t.ConstituentId)
                .Index(t => t.ContactInfoId);
            
            CreateTable(
                "dbo.ConstituentDba",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ConstituentId = c.Guid(),
                        DoingBusinessAsId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.DoingBusinessAs", t => t.DoingBusinessAsId)
                .Index(t => t.ConstituentId)
                .Index(t => t.DoingBusinessAsId);
            
            CreateTable(
                "dbo.ConstituentDemonination",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ConstituentId = c.Guid(),
                        DenominationId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.Denomination", t => t.DenominationId)
                .Index(t => t.ConstituentId)
                .Index(t => t.DenominationId);
            
            CreateTable(
                "dbo.ConstituentEducation",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ConstituentId = c.Guid(),
                        EducationId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.Education", t => t.EducationId)
                .Index(t => t.ConstituentId)
                .Index(t => t.EducationId);
            
            CreateTable(
                "dbo.ConstituentEthnicity",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ConstituentId = c.Guid(),
                        EthnicityId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.Ethnicity", t => t.EthnicityId)
                .Index(t => t.ConstituentId)
                .Index(t => t.EthnicityId);
            
            CreateTable(
                "dbo.ConstituentPaymentPreference",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ConstituentId = c.Guid(),
                        PaymentPreferenceId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.PaymentPreference", t => t.PaymentPreferenceId)
                .Index(t => t.ConstituentId)
                .Index(t => t.PaymentPreferenceId);
            
            CreateTable(
                "dbo.EducationToLevel",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        EducationId = c.Guid(),
                        EducationLevelId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Education", t => t.EducationId)
                .ForeignKey("dbo.EducationLevel", t => t.EducationLevelId)
                .Index(t => t.EducationId)
                .Index(t => t.EducationLevelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EducationToLevel", "EducationLevelId", "dbo.EducationLevel");
            DropForeignKey("dbo.EducationToLevel", "EducationId", "dbo.Education");
            DropForeignKey("dbo.ConstituentPaymentPreference", "PaymentPreferenceId", "dbo.PaymentPreference");
            DropForeignKey("dbo.ConstituentPaymentPreference", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentEthnicity", "EthnicityId", "dbo.Ethnicity");
            DropForeignKey("dbo.ConstituentEthnicity", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentEducation", "EducationId", "dbo.Education");
            DropForeignKey("dbo.ConstituentEducation", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentDemonination", "DenominationId", "dbo.Denomination");
            DropForeignKey("dbo.ConstituentDemonination", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentDba", "DoingBusinessAsId", "dbo.DoingBusinessAs");
            DropForeignKey("dbo.ConstituentDba", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentContactInfo", "ContactInfoId", "dbo.ContactInfo");
            DropForeignKey("dbo.ConstituentContactInfo", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentAlternateId", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentAlternateId", "AlternateIdId", "dbo.AlternateId");
            DropForeignKey("dbo.ConstituentAddress", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentAddress", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Constituent", "ProfessionId", "dbo.Profession");
            DropForeignKey("dbo.Constituent", "PrefixId", "dbo.Prefix");
            DropForeignKey("dbo.Prefix", "GenderId", "dbo.Gender");
            DropForeignKey("dbo.PaymentPreference", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.Constituent", "LanguageId", "dbo.Language");
            DropForeignKey("dbo.Constituent", "IncomeLevelId", "dbo.IncomeLevel");
            DropForeignKey("dbo.Constituent", "GenderId", "dbo.Gender");
            DropForeignKey("dbo.Ethnicity", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.Education", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.Education", "DegreeId", "dbo.EducationLevel");
            DropForeignKey("dbo.Constituent", "EducationLevel_Id", "dbo.EducationLevel");
            DropForeignKey("dbo.DoingBusinessAs", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.Denomination", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.ContactInfo", "ContactTypeId", "dbo.ContactType");
            DropForeignKey("dbo.ContactInfo", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.Constituent", "ConstituentTypeId", "dbo.ConstituentType");
            DropForeignKey("dbo.Constituent", "ConstituentStatusId", "dbo.ConstituentStatus");
            DropForeignKey("dbo.Constituent", "ClergyTypeId", "dbo.ClergyType");
            DropForeignKey("dbo.Constituent", "ClergyStatusId", "dbo.ClergyStatus");
            DropForeignKey("dbo.AlternateId", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.Address", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.Address", "StateId", "dbo.State");
            DropIndex("dbo.EducationToLevel", new[] { "EducationLevelId" });
            DropIndex("dbo.EducationToLevel", new[] { "EducationId" });
            DropIndex("dbo.ConstituentPaymentPreference", new[] { "PaymentPreferenceId" });
            DropIndex("dbo.ConstituentPaymentPreference", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentEthnicity", new[] { "EthnicityId" });
            DropIndex("dbo.ConstituentEthnicity", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentEducation", new[] { "EducationId" });
            DropIndex("dbo.ConstituentEducation", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentDemonination", new[] { "DenominationId" });
            DropIndex("dbo.ConstituentDemonination", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentDba", new[] { "DoingBusinessAsId" });
            DropIndex("dbo.ConstituentDba", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentContactInfo", new[] { "ContactInfoId" });
            DropIndex("dbo.ConstituentContactInfo", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentAlternateId", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentAlternateId", new[] { "AlternateIdId" });
            DropIndex("dbo.ConstituentAddress", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentAddress", new[] { "AddressId" });
            DropIndex("dbo.Prefix", new[] { "GenderId" });
            DropIndex("dbo.PaymentPreference", new[] { "Constituent_Id" });
            DropIndex("dbo.Ethnicity", new[] { "Constituent_Id" });
            DropIndex("dbo.Education", new[] { "Constituent_Id" });
            DropIndex("dbo.Education", new[] { "DegreeId" });
            DropIndex("dbo.DoingBusinessAs", new[] { "Constituent_Id" });
            DropIndex("dbo.Denomination", new[] { "Constituent_Id" });
            DropIndex("dbo.ContactInfo", new[] { "ContactTypeId" });
            DropIndex("dbo.ContactInfo", new[] { "ConstituentId" });
            DropIndex("dbo.Constituent", new[] { "EducationLevel_Id" });
            DropIndex("dbo.Constituent", new[] { "ProfessionId" });
            DropIndex("dbo.Constituent", new[] { "PrefixId" });
            DropIndex("dbo.Constituent", new[] { "LanguageId" });
            DropIndex("dbo.Constituent", new[] { "IncomeLevelId" });
            DropIndex("dbo.Constituent", new[] { "GenderId" });
            DropIndex("dbo.Constituent", new[] { "ConstituentTypeId" });
            DropIndex("dbo.Constituent", new[] { "ConstituentStatusId" });
            DropIndex("dbo.Constituent", new[] { "ClergyTypeId" });
            DropIndex("dbo.Constituent", new[] { "ClergyStatusId" });
            DropIndex("dbo.AlternateId", new[] { "ConstituentId" });
            DropIndex("dbo.Address", new[] { "Constituent_Id" });
            DropIndex("dbo.Address", new[] { "StateId" });
            DropTable("dbo.EducationToLevel");
            DropTable("dbo.ConstituentPaymentPreference");
            DropTable("dbo.ConstituentEthnicity");
            DropTable("dbo.ConstituentEducation");
            DropTable("dbo.ConstituentDemonination");
            DropTable("dbo.ConstituentDba");
            DropTable("dbo.ConstituentContactInfo");
            DropTable("dbo.ConstituentAlternateId");
            DropTable("dbo.ConstituentAddress");
            DropTable("dbo.Profession");
            DropTable("dbo.Prefix");
            DropTable("dbo.PaymentPreference");
            DropTable("dbo.Language");
            DropTable("dbo.IncomeLevel");
            DropTable("dbo.Gender");
            DropTable("dbo.Ethnicity");
            DropTable("dbo.Education");
            DropTable("dbo.EducationLevel");
            DropTable("dbo.DoingBusinessAs");
            DropTable("dbo.Denomination");
            DropTable("dbo.ContactType");
            DropTable("dbo.ContactInfo");
            DropTable("dbo.ConstituentType");
            DropTable("dbo.ConstituentStatus");
            DropTable("dbo.ClergyType");
            DropTable("dbo.ClergyStatus");
            DropTable("dbo.Constituent");
            DropTable("dbo.AlternateId");
            DropTable("dbo.State");
            DropTable("dbo.Address");
        }
    }
}

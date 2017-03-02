namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AddressLine1 = c.String(maxLength: 255),
                        AddressLine2 = c.String(maxLength: 255),
                        City = c.String(maxLength: 128),
                        CountryId = c.Guid(),
                        CountyId = c.Guid(),
                        PostalCode = c.String(maxLength: 128),
                        StateId = c.Guid(),
                        LegacyKey = c.Int(nullable: false),
                        Region1Id = c.Guid(),
                        Region2Id = c.Guid(),
                        Region3Id = c.Guid(),
                        Region4Id = c.Guid(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                        AddressType_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AddressType", t => t.AddressType_Id)
                .ForeignKey("dbo.Region", t => t.Region1Id)
                .ForeignKey("dbo.Region", t => t.Region2Id)
                .ForeignKey("dbo.Region", t => t.Region3Id)
                .ForeignKey("dbo.Region", t => t.Region4Id)
                .Index(t => t.Region1Id)
                .Index(t => t.Region2Id)
                .Index(t => t.Region3Id)
                .Index(t => t.Region4Id)
                .Index(t => t.AddressType_Id);
            
            CreateTable(
                "dbo.ConstituentAddress",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AddressId = c.Guid(),
                        ConstituentId = c.Guid(),
                        IsPrimary = c.Boolean(nullable: false),
                        Comment = c.String(maxLength: 255),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        StartDay = c.Int(nullable: false),
                        EndDay = c.Int(nullable: false),
                        ResidentType = c.Int(nullable: false),
                        AddressTypeId = c.Guid(),
                        DuplicateKey = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.AddressId)
                .ForeignKey("dbo.AddressType", t => t.AddressTypeId)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .Index(t => t.AddressId)
                .Index(t => t.ConstituentId)
                .Index(t => t.AddressTypeId)
                .Index(t => t.DuplicateKey);
            
            CreateTable(
                "dbo.AddressType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Constituent",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BirthDate = c.DateTime(storeType: "date"),
                        BirthDateType = c.Int(nullable: false),
                        BirthMonth = c.Int(),
                        BirthDay = c.Int(),
                        BirthYear = c.Int(),
                        BirthYearFrom = c.Int(),
                        BirthYearTo = c.Int(),
                        Business = c.String(maxLength: 128),
                        ClergyStatusId = c.Guid(),
                        ClergyTypeId = c.Guid(),
                        ConstituentNumber = c.Int(nullable: false),
                        ConstituentStatusId = c.Guid(),
                        ConstituentStatusDate = c.DateTime(),
                        ConstituentTypeId = c.Guid(),
                        CorrespondencePreference = c.Int(nullable: false),
                        DeceasedDate = c.DateTime(storeType: "date"),
                        DivorceDate = c.DateTime(storeType: "date"),
                        EducationLevelId = c.Guid(),
                        Employer = c.String(),
                        EmploymentEndDate = c.DateTime(storeType: "date"),
                        EmploymentStartDate = c.DateTime(storeType: "date"),
                        FirstEmploymentDate = c.DateTime(storeType: "date"),
                        FirstName = c.String(maxLength: 128),
                        FormattedName = c.String(maxLength: 255),
                        GenderId = c.Guid(),
                        IncomeLevelId = c.Guid(),
                        IsEmployee = c.Boolean(nullable: false),
                        IsIRSLetterReceived = c.Boolean(nullable: false),
                        IsTaxExempt = c.Boolean(nullable: false),
                        LanguageId = c.Guid(),
                        LastName = c.String(maxLength: 128),
                        MaritalStatusId = c.Guid(),
                        MarriageDate = c.DateTime(storeType: "date"),
                        MembershipCount = c.Int(),
                        MiddleName = c.String(maxLength: 128),
                        Name = c.String(maxLength: 255),
                        Name2 = c.String(maxLength: 128),
                        NameFormat = c.String(maxLength: 128),
                        Nickname = c.String(maxLength: 128),
                        OrdinationDate = c.DateTime(storeType: "date"),
                        PreferredPaymentMethod = c.Int(nullable: false),
                        PlaceOfOrdination = c.String(maxLength: 128),
                        Position = c.String(maxLength: 128),
                        PrefixId = c.Guid(),
                        ProfessionId = c.Guid(),
                        ProspectDate = c.DateTime(storeType: "date"),
                        SalutationType = c.Int(nullable: false),
                        Salutation = c.String(maxLength: 255),
                        Source = c.String(maxLength: 128),
                        Suffix = c.String(maxLength: 128),
                        TaxExemptVerifyDate = c.DateTime(storeType: "date"),
                        TaxId = c.String(maxLength: 128),
                        YearEstablished = c.Int(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClergyStatus", t => t.ClergyStatusId)
                .ForeignKey("dbo.ClergyType", t => t.ClergyTypeId)
                .ForeignKey("dbo.ConstituentStatus", t => t.ConstituentStatusId)
                .ForeignKey("dbo.ConstituentType", t => t.ConstituentTypeId)
                .ForeignKey("dbo.EducationLevel", t => t.EducationLevelId)
                .ForeignKey("dbo.Gender", t => t.GenderId)
                .ForeignKey("dbo.IncomeLevel", t => t.IncomeLevelId)
                .ForeignKey("dbo.Language", t => t.LanguageId)
                .ForeignKey("dbo.MaritialStatus", t => t.MaritalStatusId)
                .ForeignKey("dbo.Prefix", t => t.PrefixId)
                .ForeignKey("dbo.Profession", t => t.ProfessionId)
                .Index(t => t.ClergyStatusId)
                .Index(t => t.ClergyTypeId)
                .Index(t => t.ConstituentStatusId)
                .Index(t => t.ConstituentTypeId)
                .Index(t => t.EducationLevelId)
                .Index(t => t.GenderId)
                .Index(t => t.IncomeLevelId)
                .Index(t => t.LanguageId)
                .Index(t => t.MaritalStatusId)
                .Index(t => t.PrefixId)
                .Index(t => t.ProfessionId);
            
            CreateTable(
                "dbo.AlternateId",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConstituentId = c.Guid(),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .Index(t => t.ConstituentId);
            
            CreateTable(
                "dbo.ClergyStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClergyType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstituentStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 16),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 128),
                        BaseStatus = c.Int(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstituentType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        Category = c.Int(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        NameFormat = c.String(maxLength: 128),
                        SalutationFormal = c.String(maxLength: 128),
                        SalutationInformal = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        TagGroupId = c.Guid(),
                        Order = c.Int(nullable: false),
                        ConstituentCategory = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TagGroup", t => t.TagGroupId)
                .Index(t => t.TagGroupId);
            
            CreateTable(
                "dbo.TagGroup",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        TagSelectionType = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactInfo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Info = c.String(),
                        IsPreferred = c.Boolean(nullable: false),
                        Comment = c.String(maxLength: 255),
                        ConstituentId = c.Guid(),
                        ContactTypeId = c.Guid(),
                        ParentContactId = c.Guid(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactInfo", t => t.ParentContactId)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.ContactType", t => t.ContactTypeId)
                .Index(t => t.ConstituentId)
                .Index(t => t.ContactTypeId)
                .Index(t => t.ParentContactId);
            
            CreateTable(
                "dbo.ContactType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        ContactCategoryId = c.Guid(),
                        IsAlwaysShown = c.Boolean(nullable: false),
                        CanDelete = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactCategory", t => t.ContactCategoryId)
                .Index(t => t.ContactCategoryId);
            
            CreateTable(
                "dbo.ContactCategory",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 1),
                        Name = c.String(maxLength: 128),
                        SectionTitle = c.String(maxLength: 128),
                        TextBoxLabel = c.String(maxLength: 128),
                        DefaultContactTypeID = c.Guid(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactType", t => t.DefaultContactTypeID)
                .Index(t => t.DefaultContactTypeID);
            
            CreateTable(
                "dbo.Denomination",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        Religion = c.Int(nullable: false),
                        Affiliation = c.Int(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DoingBusinessAs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 128),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        ConstituentId = c.Guid(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .Index(t => t.ConstituentId);
            
            CreateTable(
                "dbo.EducationLevel",
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
                "dbo.Education",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Major = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        SchoolId = c.Guid(),
                        SchoolOther = c.String(maxLength: 128),
                        StartDate = c.DateTime(storeType: "date"),
                        DegreeId = c.Guid(),
                        DegreeOther = c.String(maxLength: 128),
                        EndDate = c.DateTime(storeType: "date"),
                        ConstituentId = c.Guid(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.Degree", t => t.DegreeId)
                .ForeignKey("dbo.School", t => t.SchoolId)
                .Index(t => t.SchoolId)
                .Index(t => t.DegreeId)
                .Index(t => t.ConstituentId);
            
            CreateTable(
                "dbo.Degree",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.School",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ethnicity",
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
                "dbo.Gender",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsMasculine = c.Boolean(),
                        Name = c.String(maxLength: 128),
                        Code = c.String(maxLength: 4),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncomeLevel",
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
                "dbo.Language",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 128),
                        Code = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaritialStatus",
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
                "dbo.PaymentMethod",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(maxLength: 128),
                        Category = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        StatusDate = c.DateTime(),
                        BankName = c.String(maxLength: 128),
                        BankAccount = c.String(maxLength: 64),
                        RoutingNumber = c.String(maxLength: 64),
                        AccountType = c.Int(nullable: false),
                        EFTFormatId = c.Guid(),
                        CardToken = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EFTFormat", t => t.EFTFormatId)
                .Index(t => t.EFTFormatId);
            
            CreateTable(
                "dbo.EFTFormat",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Prefix",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        LabelPrefix = c.String(maxLength: 128),
                        LabelAbbreviation = c.String(maxLength: 128),
                        Salutation = c.String(maxLength: 128),
                        ShowOnline = c.Boolean(nullable: false),
                        GenderId = c.Guid(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gender", t => t.GenderId)
                .Index(t => t.GenderId);
            
            CreateTable(
                "dbo.Profession",
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
                "dbo.Relationsihp",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RelationshipTypeId = c.Guid(),
                        Constituent1Id = c.Guid(),
                        Constituent2Id = c.Guid(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RelationshipType", t => t.RelationshipTypeId)
                .ForeignKey("dbo.Constituent", t => t.Constituent1Id)
                .ForeignKey("dbo.Constituent", t => t.Constituent2Id)
                .Index(t => t.RelationshipTypeId)
                .Index(t => t.Constituent1Id)
                .Index(t => t.Constituent2Id);
            
            CreateTable(
                "dbo.RelationshipType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        ReciprocalTypeMaleId = c.Guid(),
                        ReciprocalTypeFemaleId = c.Guid(),
                        IsSpouse = c.Boolean(nullable: false),
                        ConstituentCategory = c.Int(nullable: false),
                        RelationshipCategoryId = c.Guid(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RelationshipType", t => t.ReciprocalTypeFemaleId)
                .ForeignKey("dbo.RelationshipType", t => t.ReciprocalTypeMaleId)
                .ForeignKey("dbo.RelationshipCategory", t => t.RelationshipCategoryId)
                .Index(t => t.ReciprocalTypeMaleId)
                .Index(t => t.ReciprocalTypeFemaleId)
                .Index(t => t.RelationshipCategoryId);
            
            CreateTable(
                "dbo.RelationshipCategory",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        IsShownInQuickView = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Level = c.Int(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        ParentRegionId = c.Guid(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Region", t => t.ParentRegionId)
                .Index(t => t.ParentRegionId);
            
            CreateTable(
                "dbo.RegionArea",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Level = c.Int(nullable: false),
                        RegionId = c.Guid(),
                        CountryId = c.Guid(),
                        StateId = c.Guid(),
                        CountyId = c.Guid(),
                        City = c.String(maxLength: 128),
                        PostalCodeLow = c.String(maxLength: 128),
                        PostalCodeHigh = c.String(maxLength: 128),
                        Priority = c.Int(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Region", t => t.RegionId)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.ChangeSets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ObjectChanges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TypeName = c.String(maxLength: 128),
                        DisplayName = c.String(maxLength: 128),
                        ChangeType = c.String(maxLength: 64),
                        EntityId = c.String(maxLength: 128),
                        ChangeSetId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChangeSets", t => t.ChangeSetId, cascadeDelete: true)
                .Index(t => t.ChangeSetId);
            
            CreateTable(
                "dbo.PropertyChanges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChangeType = c.String(),
                        ObjectChangeId = c.Long(nullable: false),
                        PropertyName = c.String(maxLength: 128),
                        OriginalValue = c.String(maxLength: 512),
                        Value = c.String(maxLength: 512),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ObjectChanges", t => t.ObjectChangeId, cascadeDelete: true)
                .Index(t => t.ObjectChangeId);
            
            CreateTable(
                "dbo.Configuration",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ModuleType = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        Value = c.String(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomField",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LabelText = c.String(),
                        MinValue = c.String(),
                        MaxValue = c.String(),
                        DecimalPlaces = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(),
                        Entity = c.Int(nullable: false),
                        FieldType = c.Int(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomFieldData",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomFieldId = c.Guid(nullable: false),
                        EntityType = c.Int(nullable: false),
                        ParentEntityId = c.Guid(),
                        Value = c.String(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomField", t => t.CustomFieldId, cascadeDelete: true)
                .Index(t => t.CustomFieldId);
            
            CreateTable(
                "dbo.CustomFieldOption",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomFieldId = c.Guid(nullable: false),
                        Code = c.String(),
                        Description = c.String(),
                        SortOrder = c.Int(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomField", t => t.CustomFieldId, cascadeDelete: true)
                .Index(t => t.CustomFieldId);
            
            CreateTable(
                "dbo.FileStorage",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Extension = c.String(maxLength: 256),
                        Size = c.Long(nullable: false),
                        Data = c.Binary(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NoteCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Label = c.String(maxLength: 64),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(maxLength: 256),
                        Text = c.String(),
                        AlertStartDate = c.DateTime(storeType: "date"),
                        AlertEndDate = c.DateTime(storeType: "date"),
                        ContactDate = c.DateTime(storeType: "date"),
                        CategoryId = c.Guid(),
                        NoteCode = c.String(maxLength: 32),
                        PrimaryContactId = c.Guid(),
                        ContactMethodId = c.Guid(),
                        ParentEntityId = c.Guid(),
                        EntityType = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NoteCategories", t => t.CategoryId)
                .ForeignKey("dbo.NoteContactMethods", t => t.ContactMethodId)
                .ForeignKey("dbo.Constituent", t => t.PrimaryContactId)
                .Index(t => t.CategoryId)
                .Index(t => t.PrimaryContactId)
                .Index(t => t.ContactMethodId)
                .Index(t => t.ParentEntityId)
                .Index(t => t.EntityType);
            
            CreateTable(
                "dbo.NoteContactMethods",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 64),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NoteTopics",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 64),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RegionLevel",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Abbreviation = c.String(maxLength: 128),
                        Label = c.String(maxLength: 128),
                        Level = c.Int(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        IsChildLevel = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SectionPreference",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SectionName = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        Value = c.String(maxLength: 256),
                        IsShown = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagConstituents",
                c => new
                    {
                        Tag_Id = c.Guid(nullable: false),
                        Constituent_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.Constituent_Id })
                .ForeignKey("dbo.Tag", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.Constituent_Id);
            
            CreateTable(
                "dbo.TagConstituentTypes",
                c => new
                    {
                        Tag_Id = c.Guid(nullable: false),
                        ConstituentType_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_Id, t.ConstituentType_Id })
                .ForeignKey("dbo.Tag", t => t.Tag_Id, cascadeDelete: true)
                .ForeignKey("dbo.ConstituentType", t => t.ConstituentType_Id, cascadeDelete: true)
                .Index(t => t.Tag_Id)
                .Index(t => t.ConstituentType_Id);
            
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
            
            CreateTable(
                "dbo.PaymentMethodConstituents",
                c => new
                    {
                        PaymentMethod_Id = c.Guid(nullable: false),
                        Constituent_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PaymentMethod_Id, t.Constituent_Id })
                .ForeignKey("dbo.PaymentMethod", t => t.PaymentMethod_Id, cascadeDelete: true)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id, cascadeDelete: true)
                .Index(t => t.PaymentMethod_Id)
                .Index(t => t.Constituent_Id);
            
            CreateTable(
                "dbo.NoteTopicNotes",
                c => new
                    {
                        NoteTopic_Id = c.Guid(nullable: false),
                        Note_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.NoteTopic_Id, t.Note_Id })
                .ForeignKey("dbo.NoteTopics", t => t.NoteTopic_Id, cascadeDelete: true)
                .ForeignKey("dbo.Notes", t => t.Note_Id, cascadeDelete: true)
                .Index(t => t.NoteTopic_Id)
                .Index(t => t.Note_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "PrimaryContactId", "dbo.Constituent");
            DropForeignKey("dbo.NoteTopicNotes", "Note_Id", "dbo.Notes");
            DropForeignKey("dbo.NoteTopicNotes", "NoteTopic_Id", "dbo.NoteTopics");
            DropForeignKey("dbo.Notes", "ContactMethodId", "dbo.NoteContactMethods");
            DropForeignKey("dbo.Notes", "CategoryId", "dbo.NoteCategories");
            DropForeignKey("dbo.CustomFieldOption", "CustomFieldId", "dbo.CustomField");
            DropForeignKey("dbo.CustomFieldData", "CustomFieldId", "dbo.CustomField");
            DropForeignKey("dbo.PropertyChanges", "ObjectChangeId", "dbo.ObjectChanges");
            DropForeignKey("dbo.ObjectChanges", "ChangeSetId", "dbo.ChangeSets");
            DropForeignKey("dbo.RegionArea", "RegionId", "dbo.Region");
            DropForeignKey("dbo.Address", "Region4Id", "dbo.Region");
            DropForeignKey("dbo.Address", "Region3Id", "dbo.Region");
            DropForeignKey("dbo.Address", "Region2Id", "dbo.Region");
            DropForeignKey("dbo.Address", "Region1Id", "dbo.Region");
            DropForeignKey("dbo.Region", "ParentRegionId", "dbo.Region");
            DropForeignKey("dbo.Relationsihp", "Constituent2Id", "dbo.Constituent");
            DropForeignKey("dbo.Relationsihp", "Constituent1Id", "dbo.Constituent");
            DropForeignKey("dbo.Relationsihp", "RelationshipTypeId", "dbo.RelationshipType");
            DropForeignKey("dbo.RelationshipType", "RelationshipCategoryId", "dbo.RelationshipCategory");
            DropForeignKey("dbo.RelationshipType", "ReciprocalTypeMaleId", "dbo.RelationshipType");
            DropForeignKey("dbo.RelationshipType", "ReciprocalTypeFemaleId", "dbo.RelationshipType");
            DropForeignKey("dbo.Constituent", "ProfessionId", "dbo.Profession");
            DropForeignKey("dbo.Constituent", "PrefixId", "dbo.Prefix");
            DropForeignKey("dbo.Prefix", "GenderId", "dbo.Gender");
            DropForeignKey("dbo.PaymentMethod", "EFTFormatId", "dbo.EFTFormat");
            DropForeignKey("dbo.PaymentMethodConstituents", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.PaymentMethodConstituents", "PaymentMethod_Id", "dbo.PaymentMethod");
            DropForeignKey("dbo.Constituent", "MaritalStatusId", "dbo.MaritialStatus");
            DropForeignKey("dbo.Constituent", "LanguageId", "dbo.Language");
            DropForeignKey("dbo.Constituent", "IncomeLevelId", "dbo.IncomeLevel");
            DropForeignKey("dbo.Constituent", "GenderId", "dbo.Gender");
            DropForeignKey("dbo.EthnicityConstituents", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.EthnicityConstituents", "Ethnicity_Id", "dbo.Ethnicity");
            DropForeignKey("dbo.Education", "SchoolId", "dbo.School");
            DropForeignKey("dbo.Education", "DegreeId", "dbo.Degree");
            DropForeignKey("dbo.Education", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.Constituent", "EducationLevelId", "dbo.EducationLevel");
            DropForeignKey("dbo.DoingBusinessAs", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.DenominationConstituents", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.DenominationConstituents", "Denomination_Id", "dbo.Denomination");
            DropForeignKey("dbo.ContactInfo", "ContactTypeId", "dbo.ContactType");
            DropForeignKey("dbo.ContactCategory", "DefaultContactTypeID", "dbo.ContactType");
            DropForeignKey("dbo.ContactType", "ContactCategoryId", "dbo.ContactCategory");
            DropForeignKey("dbo.ContactInfo", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ContactInfo", "ParentContactId", "dbo.ContactInfo");
            DropForeignKey("dbo.Constituent", "ConstituentTypeId", "dbo.ConstituentType");
            DropForeignKey("dbo.Tag", "TagGroupId", "dbo.TagGroup");
            DropForeignKey("dbo.TagConstituentTypes", "ConstituentType_Id", "dbo.ConstituentType");
            DropForeignKey("dbo.TagConstituentTypes", "Tag_Id", "dbo.Tag");
            DropForeignKey("dbo.TagConstituents", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.TagConstituents", "Tag_Id", "dbo.Tag");
            DropForeignKey("dbo.Constituent", "ConstituentStatusId", "dbo.ConstituentStatus");
            DropForeignKey("dbo.ConstituentAddress", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.Constituent", "ClergyTypeId", "dbo.ClergyType");
            DropForeignKey("dbo.Constituent", "ClergyStatusId", "dbo.ClergyStatus");
            DropForeignKey("dbo.AlternateId", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentAddress", "AddressTypeId", "dbo.AddressType");
            DropForeignKey("dbo.Address", "AddressType_Id", "dbo.AddressType");
            DropForeignKey("dbo.ConstituentAddress", "AddressId", "dbo.Address");
            DropIndex("dbo.NoteTopicNotes", new[] { "Note_Id" });
            DropIndex("dbo.NoteTopicNotes", new[] { "NoteTopic_Id" });
            DropIndex("dbo.PaymentMethodConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.PaymentMethodConstituents", new[] { "PaymentMethod_Id" });
            DropIndex("dbo.EthnicityConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.EthnicityConstituents", new[] { "Ethnicity_Id" });
            DropIndex("dbo.DenominationConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.DenominationConstituents", new[] { "Denomination_Id" });
            DropIndex("dbo.TagConstituentTypes", new[] { "ConstituentType_Id" });
            DropIndex("dbo.TagConstituentTypes", new[] { "Tag_Id" });
            DropIndex("dbo.TagConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.TagConstituents", new[] { "Tag_Id" });
            DropIndex("dbo.Notes", new[] { "EntityType" });
            DropIndex("dbo.Notes", new[] { "ParentEntityId" });
            DropIndex("dbo.Notes", new[] { "ContactMethodId" });
            DropIndex("dbo.Notes", new[] { "PrimaryContactId" });
            DropIndex("dbo.Notes", new[] { "CategoryId" });
            DropIndex("dbo.CustomFieldOption", new[] { "CustomFieldId" });
            DropIndex("dbo.CustomFieldData", new[] { "CustomFieldId" });
            DropIndex("dbo.PropertyChanges", new[] { "ObjectChangeId" });
            DropIndex("dbo.ObjectChanges", new[] { "ChangeSetId" });
            DropIndex("dbo.RegionArea", new[] { "RegionId" });
            DropIndex("dbo.Region", new[] { "ParentRegionId" });
            DropIndex("dbo.RelationshipType", new[] { "RelationshipCategoryId" });
            DropIndex("dbo.RelationshipType", new[] { "ReciprocalTypeFemaleId" });
            DropIndex("dbo.RelationshipType", new[] { "ReciprocalTypeMaleId" });
            DropIndex("dbo.Relationsihp", new[] { "Constituent2Id" });
            DropIndex("dbo.Relationsihp", new[] { "Constituent1Id" });
            DropIndex("dbo.Relationsihp", new[] { "RelationshipTypeId" });
            DropIndex("dbo.Prefix", new[] { "GenderId" });
            DropIndex("dbo.PaymentMethod", new[] { "EFTFormatId" });
            DropIndex("dbo.Education", new[] { "ConstituentId" });
            DropIndex("dbo.Education", new[] { "DegreeId" });
            DropIndex("dbo.Education", new[] { "SchoolId" });
            DropIndex("dbo.DoingBusinessAs", new[] { "ConstituentId" });
            DropIndex("dbo.ContactCategory", new[] { "DefaultContactTypeID" });
            DropIndex("dbo.ContactType", new[] { "ContactCategoryId" });
            DropIndex("dbo.ContactInfo", new[] { "ParentContactId" });
            DropIndex("dbo.ContactInfo", new[] { "ContactTypeId" });
            DropIndex("dbo.ContactInfo", new[] { "ConstituentId" });
            DropIndex("dbo.Tag", new[] { "TagGroupId" });
            DropIndex("dbo.AlternateId", new[] { "ConstituentId" });
            DropIndex("dbo.Constituent", new[] { "ProfessionId" });
            DropIndex("dbo.Constituent", new[] { "PrefixId" });
            DropIndex("dbo.Constituent", new[] { "MaritalStatusId" });
            DropIndex("dbo.Constituent", new[] { "LanguageId" });
            DropIndex("dbo.Constituent", new[] { "IncomeLevelId" });
            DropIndex("dbo.Constituent", new[] { "GenderId" });
            DropIndex("dbo.Constituent", new[] { "EducationLevelId" });
            DropIndex("dbo.Constituent", new[] { "ConstituentTypeId" });
            DropIndex("dbo.Constituent", new[] { "ConstituentStatusId" });
            DropIndex("dbo.Constituent", new[] { "ClergyTypeId" });
            DropIndex("dbo.Constituent", new[] { "ClergyStatusId" });
            DropIndex("dbo.ConstituentAddress", new[] { "DuplicateKey" });
            DropIndex("dbo.ConstituentAddress", new[] { "AddressTypeId" });
            DropIndex("dbo.ConstituentAddress", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentAddress", new[] { "AddressId" });
            DropIndex("dbo.Address", new[] { "AddressType_Id" });
            DropIndex("dbo.Address", new[] { "Region4Id" });
            DropIndex("dbo.Address", new[] { "Region3Id" });
            DropIndex("dbo.Address", new[] { "Region2Id" });
            DropIndex("dbo.Address", new[] { "Region1Id" });
            DropTable("dbo.NoteTopicNotes");
            DropTable("dbo.PaymentMethodConstituents");
            DropTable("dbo.EthnicityConstituents");
            DropTable("dbo.DenominationConstituents");
            DropTable("dbo.TagConstituentTypes");
            DropTable("dbo.TagConstituents");
            DropTable("dbo.SectionPreference");
            DropTable("dbo.RegionLevel");
            DropTable("dbo.NoteTopics");
            DropTable("dbo.NoteContactMethods");
            DropTable("dbo.Notes");
            DropTable("dbo.NoteCategories");
            DropTable("dbo.LogEntry");
            DropTable("dbo.FileStorage");
            DropTable("dbo.CustomFieldOption");
            DropTable("dbo.CustomFieldData");
            DropTable("dbo.CustomField");
            DropTable("dbo.Configuration");
            DropTable("dbo.PropertyChanges");
            DropTable("dbo.ObjectChanges");
            DropTable("dbo.ChangeSets");
            DropTable("dbo.RegionArea");
            DropTable("dbo.Region");
            DropTable("dbo.RelationshipCategory");
            DropTable("dbo.RelationshipType");
            DropTable("dbo.Relationsihp");
            DropTable("dbo.Profession");
            DropTable("dbo.Prefix");
            DropTable("dbo.EFTFormat");
            DropTable("dbo.PaymentMethod");
            DropTable("dbo.MaritialStatus");
            DropTable("dbo.Language");
            DropTable("dbo.IncomeLevel");
            DropTable("dbo.Gender");
            DropTable("dbo.Ethnicity");
            DropTable("dbo.School");
            DropTable("dbo.Degree");
            DropTable("dbo.Education");
            DropTable("dbo.EducationLevel");
            DropTable("dbo.DoingBusinessAs");
            DropTable("dbo.Denomination");
            DropTable("dbo.ContactCategory");
            DropTable("dbo.ContactType");
            DropTable("dbo.ContactInfo");
            DropTable("dbo.TagGroup");
            DropTable("dbo.Tag");
            DropTable("dbo.ConstituentType");
            DropTable("dbo.ConstituentStatus");
            DropTable("dbo.ClergyType");
            DropTable("dbo.ClergyStatus");
            DropTable("dbo.AlternateId");
            DropTable("dbo.Constituent");
            DropTable("dbo.AddressType");
            DropTable("dbo.ConstituentAddress");
            DropTable("dbo.Address");
        }
    }
}

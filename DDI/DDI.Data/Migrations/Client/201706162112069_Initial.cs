namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    using DDI.Data.Extensions;
    using DDI.Data.Statics;

    public partial class Initial : DbMigration
    {

        public override void Up()
        {
            CreateFileGroup();
            CreateFileStorageTable();
            CreateSequences();

            CreateTable(
                "dbo.Attachment",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(maxLength: 256),
                        FileId = c.Guid(),
                        NoteId = c.Guid(),
                        ParentEntityId = c.Guid(),
                        EntityType = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileStorage", t => t.FileId)
                .ForeignKey("dbo.Note", t => t.NoteId)
                .Index(t => t.FileId)
                .Index(t => t.NoteId)
                .Index(t => t.ParentEntityId)
                .Index(t => t.EntityType);
            
            //CreateTable(
            //    "dbo.FileStorage",
            //    c => new
            //        {
            //            Id = c.Guid(nullable: false),
            //            Name = c.String(maxLength: 256),
            //            Extension = c.String(maxLength: 8),
            //            Size = c.Long(nullable: false),
            //            Data = c.Binary(),
            //            CreatedBy = c.String(maxLength: 64),
            //            CreatedOn = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Note",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(maxLength: 256),
                        Text = c.String(),
                        AlertStartDate = c.DateTime(storeType: "date"),
                        AlertEndDate = c.DateTime(storeType: "date"),
                        ContactDate = c.DateTime(storeType: "date"),
                        CategoryId = c.Guid(),
                        NoteCodeId = c.Guid(),
                        PrimaryContactId = c.Guid(),
                        ContactMethodId = c.Guid(),
                        UserResponsibleId = c.Guid(),
                        ParentEntityId = c.Guid(),
                        EntityType = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NoteCategory", t => t.CategoryId)
                .ForeignKey("dbo.NoteContactMethod", t => t.ContactMethodId)
                .ForeignKey("dbo.NoteCode", t => t.NoteCodeId)
                .ForeignKey("dbo.Constituent", t => t.PrimaryContactId)
                .ForeignKey("dbo.Users", t => t.UserResponsibleId)
                .Index(t => t.CategoryId)
                .Index(t => t.NoteCodeId)
                .Index(t => t.PrimaryContactId)
                .Index(t => t.ContactMethodId)
                .Index(t => t.UserResponsibleId)
                .Index(t => t.ParentEntityId)
                .Index(t => t.EntityType);
            
            CreateTable(
                "dbo.NoteCategory",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Label = c.String(maxLength: 64),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NoteContactMethod",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 64),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.NoteCode",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.NoteTopic",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 64),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Constituent",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BirthDateType = c.Int(nullable: false),
                        BirthMonth = c.Int(),
                        BirthDay = c.Int(),
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.ClergyType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                        AddressType_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Region", t => t.Region1Id)
                .ForeignKey("dbo.Region", t => t.Region2Id)
                .ForeignKey("dbo.Region", t => t.Region3Id)
                .ForeignKey("dbo.Region", t => t.Region4Id)
                .ForeignKey("dbo.AddressType", t => t.AddressType_Id)
                .Index(t => t.Region1Id)
                .Index(t => t.Region2Id)
                .Index(t => t.Region3Id)
                .Index(t => t.Region4Id)
                .Index(t => t.AddressType_Id);
            
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Level = c.Int(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        ParentRegionId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Region", t => t.ParentRegionId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true)
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Region", t => t.RegionId)
                .Index(t => t.RegionId);
            
            CreateTable(
                "dbo.AddressType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.ConstituentStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        BaseStatus = c.Int(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TagGroup", t => t.TagGroupId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true)
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactCategory", t => t.ContactCategoryId)
                .Index(t => new { t.ContactCategoryId, t.Code }, unique: true, name: "IX_Code")
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.ContactCategory",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 1),
                        Name = c.String(maxLength: 128),
                        SectionTitle = c.String(maxLength: 128),
                        TextBoxLabel = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        DefaultContactTypeId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactType", t => t.DefaultContactTypeId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.DefaultContactTypeId);
            
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.DoingBusinessAs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 128),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        ConstituentId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
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
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Education",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Major = c.String(maxLength: 128),
                        SchoolId = c.Guid(),
                        SchoolOther = c.String(maxLength: 128),
                        StartDate = c.DateTime(storeType: "date"),
                        DegreeId = c.Guid(),
                        DegreeOther = c.String(maxLength: 128),
                        EndDate = c.DateTime(storeType: "date"),
                        ConstituentId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.School",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Ethnicity",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Gender",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsMasculine = c.Boolean(),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.IncomeLevel",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.InvestmentRelationship",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConstituentId = c.Guid(),
                        InvestmentId = c.Guid(),
                        InvestmentRelationshipType = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.Investment", t => t.InvestmentId)
                .Index(t => t.ConstituentId)
                .Index(t => t.InvestmentId);
            
            CreateTable(
                "dbo.Investment",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BusinessUnitId = c.Guid(),
                        CUSIP = c.String(),
                        InvestmentDescription = c.String(maxLength: 256),
                        InvestmentOwnershipTypeId = c.Guid(),
                        InvestmentNumber = c.Int(nullable: false),
                        InvestmentStatus = c.Int(nullable: false),
                        InvestmentStatusDate = c.DateTime(),
                        InvestmentTypeId = c.Guid(),
                        CurrentMaturityDate = c.DateTime(storeType: "date"),
                        OriginalMaturityDate = c.DateTime(storeType: "date"),
                        MaturityMethod = c.Int(nullable: false),
                        LastMaturityDate = c.DateTime(),
                        MaturityResponseDate = c.DateTime(),
                        NumberOfRenewals = c.Int(nullable: false),
                        PurchaseDate = c.DateTime(storeType: "date"),
                        IssuanceMethod = c.Int(nullable: false),
                        OriginalPurchaseAmount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Rate = c.Decimal(nullable: false, precision: 6, scale: 4),
                        StepUpEligible = c.Boolean(nullable: false),
                        StepUpDate = c.DateTime(),
                        InterestFrequency = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                        RenewalInvestmentType_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnitId)
                .ForeignKey("dbo.InvestmentOwnershipType", t => t.InvestmentOwnershipTypeId)
                .ForeignKey("dbo.InvestmentType", t => t.InvestmentTypeId)
                .ForeignKey("dbo.InvestmentType", t => t.RenewalInvestmentType_Id)
                .Index(t => t.BusinessUnitId)
                .Index(t => t.InvestmentOwnershipTypeId)
                .Index(t => t.InvestmentTypeId)
                .Index(t => t.RenewalInvestmentType_Id);
            
            CreateTable(
                "dbo.BusinessUnit",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 128),
                        BusinessUnitType = c.Int(nullable: false),
                        Code = c.String(maxLength: 16),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.BusinessUnitFromTo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        BusinessUnitId = c.Guid(),
                        OffsettingBusinessUnitId = c.Guid(),
                        FromAccountId = c.Guid(),
                        ToAccountId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccount", t => t.FromAccountId)
                .ForeignKey("dbo.BusinessUnit", t => t.OffsettingBusinessUnitId)
                .ForeignKey("dbo.LedgerAccount", t => t.ToAccountId)
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnitId)
                .Index(t => new { t.FiscalYearId, t.BusinessUnitId, t.OffsettingBusinessUnitId }, unique: true, name: "IX_FiscalYear_BUs")
                .Index(t => t.FromAccountId)
                .Index(t => t.ToAccountId);
            
            CreateTable(
                "dbo.FiscalYear",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LedgerId = c.Guid(),
                        Name = c.String(maxLength: 16),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        Status = c.Int(nullable: false),
                        NumberOfPeriods = c.Int(nullable: false),
                        CurrentPeriodNumber = c.Int(nullable: false),
                        HasAdjustmentPeriod = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ledger", t => t.LedgerId)
                .Index(t => new { t.LedgerId, t.Name }, unique: true, name: "IX_Name");
            
            CreateTable(
                "dbo.FiscalPeriod",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        PeriodNumber = c.Int(nullable: false),
                        StartDate = c.DateTime(storeType: "date"),
                        EndDate = c.DateTime(storeType: "date"),
                        IsAdjustmentPeriod = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .Index(t => new { t.FiscalYearId, t.PeriodNumber }, unique: true, name: "IX_FiscalYear_PeriodNumber");
            
            CreateTable(
                "dbo.Ledger",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DefaultFiscalYearId = c.Guid(),
                        IsParent = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 16),
                        Name = c.String(maxLength: 128),
                        NumberOfSegments = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        FixedBudgetName = c.String(maxLength: 40),
                        WorkingBudgetName = c.String(maxLength: 40),
                        WhatIfBudgetName = c.String(maxLength: 40),
                        ApproveJournals = c.Boolean(nullable: false),
                        FundAccounting = c.Boolean(nullable: false),
                        BusinessUnitId = c.Guid(),
                        PostAutomatically = c.Boolean(nullable: false),
                        PostDaysInAdvance = c.Int(nullable: false),
                        OrgLedgerId = c.Guid(),
                        PriorPeriodPostingMode = c.Int(nullable: false),
                        CapitalizeHeaders = c.Boolean(nullable: false),
                        CopyCOAChanges = c.Boolean(nullable: false),
                        AccountGroupLevels = c.Int(nullable: false),
                        AccountGroup1Title = c.String(maxLength: 40),
                        AccountGroup2Title = c.String(maxLength: 40),
                        AccountGroup3Title = c.String(maxLength: 40),
                        AccountGroup4Title = c.String(maxLength: 40),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnitId)
                .ForeignKey("dbo.FiscalYear", t => t.DefaultFiscalYearId)
                .ForeignKey("dbo.Ledger", t => t.OrgLedgerId)
                .Index(t => t.DefaultFiscalYearId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.BusinessUnitId)
                .Index(t => t.OrgLedgerId);
            
            CreateTable(
                "dbo.LedgerAccount",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LedgerId = c.Guid(),
                        AccountNumber = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ledger", t => t.LedgerId)
                .Index(t => t.LedgerId);
            
            CreateTable(
                "dbo.LedgerAccountYear",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LedgerAccountId = c.Guid(),
                        FiscalYearId = c.Guid(),
                        AccountId = c.Guid(),
                        IsMerge = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccount", t => t.LedgerAccountId)
                .Index(t => t.LedgerAccountId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        AccountNumber = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        BeginningBalance = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Category = c.Int(nullable: false),
                        IsNormallyDebit = c.Boolean(nullable: false),
                        SortKey = c.String(maxLength: 128),
                        ClosingAccountId = c.Guid(),
                        Group1Id = c.Guid(),
                        Group2Id = c.Guid(),
                        Group3Id = c.Guid(),
                        Group4Id = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.ClosingAccountId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.AccountGroup", t => t.Group1Id)
                .ForeignKey("dbo.AccountGroup", t => t.Group2Id)
                .ForeignKey("dbo.AccountGroup", t => t.Group3Id)
                .ForeignKey("dbo.AccountGroup", t => t.Group4Id)
                .Index(t => t.FiscalYearId)
                .Index(t => t.SortKey)
                .Index(t => t.ClosingAccountId)
                .Index(t => t.Group1Id)
                .Index(t => t.Group2Id)
                .Index(t => t.Group3Id)
                .Index(t => t.Group4Id);            
            
            CreateTable(
                "dbo.AccountSegment",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Level = c.Int(nullable: false),
                        AccountId = c.Guid(),
                        SegmentId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .ForeignKey("dbo.Segment", t => t.SegmentId)
                .Index(t => t.AccountId)
                .Index(t => t.SegmentId);
            
            CreateTable(
                "dbo.Segment",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        SegmentLevelId = c.Guid(),
                        Level = c.Int(nullable: false),
                        Code = c.String(maxLength: 30),
                        Name = c.String(maxLength: 128),
                        ParentSegmentId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Segment", t => t.ParentSegmentId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.SegmentLevel", t => t.SegmentLevelId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.SegmentLevelId)
                .Index(t => t.ParentSegmentId);
            
            CreateTable(
                "dbo.SegmentLevel",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LedgerId = c.Guid(),
                        Level = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Format = c.Int(nullable: false),
                        Length = c.Int(nullable: false),
                        IsLinked = c.Boolean(nullable: false),
                        IsCommon = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 40),
                        Abbreviation = c.String(maxLength: 16),
                        Separator = c.String(maxLength: 1),
                        SortOrder = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ledger", t => t.LedgerId)
                .Index(t => new { t.LedgerId, t.Level }, unique: true, name: "IX_Level")
                .Index(t => new { t.LedgerId, t.Name }, unique: true, name: "IX_Name");
            
            CreateTable(
                "dbo.AccountBudget",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountId = c.Guid(),
                        BudgetType = c.Int(nullable: false),
                        YearAmount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount01 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount02 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount03 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount04 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount05 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount06 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount07 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount08 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount09 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount10 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount11 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount12 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount13 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Budget_Amount14 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount01 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount02 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount03 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount04 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount05 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount06 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount07 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount08 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount09 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount10 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount11 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount12 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount13 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent_Amount14 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .Index(t => new { t.AccountId, t.BudgetType }, unique: true, name: "IX_Account_BudgetType");
            
            CreateTable(
                "dbo.AccountGroup",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 128),
                        Sequence = c.Int(),
                        FiscalYearId = c.Guid(),
                        Category = c.Int(nullable: false),
                        ParentGroupId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountGroup", t => t.ParentGroupId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.ParentGroupId);
            
            CreateTable(
                "dbo.AccountPriorYear",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountId = c.Guid(),
                        PriorAccountId = c.Guid(),
                        Percentage = c.Decimal(nullable: false, precision: 5, scale: 2),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .ForeignKey("dbo.Account", t => t.PriorAccountId)
                .Index(t => t.AccountId)
                .Index(t => t.PriorAccountId);
            
            CreateTable(
                "dbo.PostedTransaction",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TransactionNumber = c.Long(nullable: false),
                        LedgerAccountYearId = c.Guid(),
                        FiscalYearId = c.Guid(),
                        PeriodNumber = c.Int(nullable: false),
                        PostedTransactionType = c.Int(nullable: false),
                        TransactionDate = c.DateTime(storeType: "date"),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        LineNumber = c.Int(nullable: false),
                        Description = c.String(maxLength: 255),
                        TransactionType = c.Int(nullable: false),
                        IsAdjustment = c.Boolean(nullable: false),
                        TransactionId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccountYear", t => t.LedgerAccountYearId)
                .ForeignKey("dbo.Transaction", t => t.TransactionId)
                .Index(t => t.LedgerAccountYearId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.TransactionId);
            
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        TransactionNumber = c.Long(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        TransactionDate = c.DateTime(storeType: "date"),
                        PostDate = c.DateTime(),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        DebitAccountId = c.Guid(),
                        CreditAccountId = c.Guid(),
                        Status = c.Int(nullable: false),
                        IsAdjustment = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 255),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LedgerAccountYear", t => t.CreditAccountId)
                .ForeignKey("dbo.LedgerAccountYear", t => t.DebitAccountId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.DebitAccountId)
                .Index(t => t.CreditAccountId);
            
            CreateTable(
                "dbo.EntityTransaction",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ParentEntityId = c.Guid(),
                        EntityType = c.String(maxLength: 128),
                        Relationship = c.Int(nullable: false),
                        Category = c.Int(nullable: false),
                        AmountType = c.Int(nullable: false),
                        TransactionId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transaction", t => t.TransactionId)
                .Index(t => t.ParentEntityId)
                .Index(t => t.EntityType)
                .Index(t => t.TransactionId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false, identity: true),
                        UserName = c.String(maxLength: 256),
                        FullName = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                        DefaultBusinessUnitId = c.Guid(),
                        ConstituentId = c.Guid(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.BusinessUnit", t => t.DefaultBusinessUnitId)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.DefaultBusinessUnitId)
                .Index(t => t.ConstituentId);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        Module = c.String(maxLength: 64),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.InvestmentInterestPayout",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InvestmentId = c.Guid(),
                        Priority = c.Int(nullable: false),
                        InterestPaymentMethod = c.Int(nullable: false),
                        ConstituentId = c.Guid(),
                        Percent = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.Investment", t => t.InvestmentId)
                .Index(t => t.InvestmentId)
                .Index(t => t.ConstituentId);
            
            CreateTable(
                "dbo.InvestmentOwnershipType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        NumericValue = c.Int(),
                        DefaultSelect = c.Int(),
                        TextValue = c.String(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.InvestmentType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.MaritialStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
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
                        IsActive = c.Boolean(nullable: false),
                        ShowOnline = c.Boolean(nullable: false),
                        GenderId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gender", t => t.GenderId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.GenderId);
            
            CreateTable(
                "dbo.Profession",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 128),
                        Code = c.String(maxLength: 4),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true)
                .Index(t => t.Code, unique: true);
            
            CreateTable(
                "dbo.Relationship",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RelationshipTypeId = c.Guid(),
                        Constituent1Id = c.Guid(),
                        Constituent2Id = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RelationshipType", t => t.ReciprocalTypeFemaleId)
                .ForeignKey("dbo.RelationshipType", t => t.ReciprocalTypeMaleId)
                .ForeignKey("dbo.RelationshipCategory", t => t.RelationshipCategoryId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true)
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChangeSets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        UserName = c.String(),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
                        PropertyTypeName = c.String(maxLength: 128),
                        OriginalDisplayName = c.String(maxLength: 512),
                        OriginalValue = c.String(maxLength: 512),
                        NewValue = c.String(maxLength: 512),
                        NewDisplayName = c.String(maxLength: 512),
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstituentPicture",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConstituentId = c.Guid(nullable: false),
                        FileId = c.Guid(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId, cascadeDelete: true)
                .ForeignKey("dbo.FileStorage", t => t.FileId, cascadeDelete: true)
                .Index(t => t.ConstituentId)
                .Index(t => t.FileId);
            
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
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomField",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LabelText = c.String(maxLength: 128),
                        MinValue = c.String(maxLength: 64),
                        MaxValue = c.String(maxLength: 64),
                        DecimalPlaces = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(),
                        Entity = c.Int(nullable: false),
                        FieldType = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomFieldData",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EntityType = c.Int(nullable: false),
                        ParentEntityId = c.Guid(),
                        Value = c.String(),
                        CustomFieldId = c.Guid(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
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
                        Code = c.String(maxLength: 16),
                        Description = c.String(maxLength: 256),
                        SortOrder = c.Int(nullable: false),
                        CustomFieldId = c.Guid(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomField", t => t.CustomFieldId, cascadeDelete: true)
                .Index(t => t.CustomFieldId);
            
            CreateTable(
                "dbo.EntityApproval",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ParentEntityId = c.Guid(),
                        EntityType = c.String(maxLength: 128),
                        ApprovedById = c.Guid(),
                        ApprovedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ApprovedById)
                .Index(t => t.ParentEntityId)
                .Index(t => t.EntityType)
                .Index(t => t.ApprovedById);
            
            CreateTable(
                "dbo.EntityMapping",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MappingType = c.Int(nullable: false),
                        PropertyName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EntityNumber",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EntityNumberType = c.Int(nullable: false),
                        RangeId = c.Guid(),
                        NextNumber = c.Int(nullable: false),
                        PreviousNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccountClose",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountId = c.Guid(),
                        Debit_Amount01 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount02 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount03 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount04 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount05 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount06 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount07 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount08 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount09 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount10 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount11 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount12 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount13 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Debit_Amount14 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount01 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount02 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount03 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount04 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount05 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount06 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount07 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount08 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount09 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount10 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount11 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount12 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount13 = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Credit_Amount14 = c.Decimal(nullable: false, precision: 14, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.FundFromTo",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        FundId = c.Guid(),
                        OffsettingFundId = c.Guid(),
                        FromAccountId = c.Guid(),
                        ToAccountId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccount", t => t.FromAccountId)
                .ForeignKey("dbo.Fund", t => t.FundId)
                .ForeignKey("dbo.Fund", t => t.OffsettingFundId)
                .ForeignKey("dbo.LedgerAccount", t => t.ToAccountId)
                .Index(t => new { t.FiscalYearId, t.FundId, t.OffsettingFundId }, unique: true, name: "IX_FiscalYear_Funds")
                .Index(t => t.FromAccountId)
                .Index(t => t.ToAccountId);
            
            CreateTable(
                "dbo.Fund",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        FundSegmentId = c.Guid(),
                        FundBalanceAccountId = c.Guid(),
                        ClosingRevenueAccountId = c.Guid(),
                        ClosingExpenseAccountId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LedgerAccount", t => t.ClosingExpenseAccountId)
                .ForeignKey("dbo.LedgerAccount", t => t.ClosingRevenueAccountId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccount", t => t.FundBalanceAccountId)
                .ForeignKey("dbo.Segment", t => t.FundSegmentId)
                .Index(t => new { t.FiscalYearId, t.FundSegmentId }, unique: true, name: "IX_FiscalYear_FundSegment")
                .Index(t => t.FundBalanceAccountId)
                .Index(t => t.ClosingRevenueAccountId)
                .Index(t => t.ClosingExpenseAccountId);
            
            CreateTable(
                "dbo.JournalLine",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        Comment = c.String(maxLength: 255),
                        LedgerAccountId = c.Guid(),
                        TransactionDate = c.DateTime(storeType: "date"),
                        DeletedOn = c.DateTime(storeType: "date"),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent = c.Decimal(nullable: false, precision: 5, scale: 2),
                        DueToMode = c.Int(nullable: false),
                        SourceBusinessUnitId = c.Guid(),
                        SourceFundId = c.Guid(),
                        JournalId = c.Guid(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Journal", t => t.JournalId, cascadeDelete: true)
                .ForeignKey("dbo.LedgerAccount", t => t.LedgerAccountId)
                .ForeignKey("dbo.BusinessUnit", t => t.SourceBusinessUnitId)
                .ForeignKey("dbo.Fund", t => t.SourceFundId)
                .Index(t => t.LedgerAccountId)
                .Index(t => t.SourceBusinessUnitId)
                .Index(t => t.SourceFundId)
                .Index(t => t.JournalId);
            
            CreateTable(
                "dbo.Journal",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        JournalNumber = c.Int(nullable: false),
                        JournalType = c.Int(nullable: false),
                        BusinessUnitId = c.Guid(),
                        FiscalYearId = c.Guid(),
                        Comment = c.String(maxLength: 255),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        TransactionDate = c.DateTime(storeType: "date"),
                        ReverseOnDate = c.DateTime(storeType: "date"),
                        IsReversed = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(storeType: "date"),
                        RecurringType = c.Int(nullable: false),
                        RecurringDay = c.Int(nullable: false),
                        PreviousDate = c.DateTime(storeType: "date"),
                        IsExpired = c.Boolean(nullable: false),
                        ExpireAmount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        ExpireAmountTotal = c.Decimal(nullable: false, precision: 14, scale: 2),
                        ExpireDate = c.DateTime(storeType: "date"),
                        ExpireCount = c.Int(nullable: false),
                        ExpireCountTotal = c.Int(nullable: false),
                        ParentJournalId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnitId)
                .ForeignKey("dbo.Journal", t => t.ParentJournalId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .Index(t => t.BusinessUnitId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.ParentJournalId);
            
            CreateTable(
                "dbo.LedgerAccountMerge",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FromAccountId = c.Guid(),
                        ToAccountId = c.Guid(),
                        FromAccountNumber = c.String(maxLength: 128),
                        ToAccountNumber = c.String(maxLength: 128),
                        FiscalYearId = c.Guid(),
                        MergedById = c.Guid(),
                        MergedOn = c.DateTime(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .ForeignKey("dbo.LedgerAccount", t => t.FromAccountId)
                .ForeignKey("dbo.Users", t => t.MergedById)
                .ForeignKey("dbo.LedgerAccount", t => t.ToAccountId)
                .Index(t => t.FromAccountId)
                .Index(t => t.ToAccountId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.MergedById);
            
            CreateTable(
                "dbo.SavedEntityMapping",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 128),
                        Description = c.String(maxLength: 256),
                        MappingType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.SavedEntityMappingField",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SavedEntityMappingId = c.Guid(nullable: false),
                        EntityMappingId = c.Guid(nullable: false),
                        ColumnName = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntityMapping", t => t.EntityMappingId, cascadeDelete: true)
                .ForeignKey("dbo.SavedEntityMapping", t => t.SavedEntityMappingId, cascadeDelete: true)
                .Index(t => t.SavedEntityMappingId)
                .Index(t => t.EntityMappingId);
            
            CreateTable(
                "dbo.SectionPreference",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SectionName = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        Value = c.String(maxLength: 256),
                        IsShown = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransactionXref",
                c => new
                    {
                        PostedTransactionId = c.Guid(nullable: false),
                        TransactionId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostedTransactionId, t.TransactionId });
            
            CreateTable(
                "dbo.NoteTopicNotes",
                c => new
                    {
                        NoteTopic_Id = c.Guid(nullable: false),
                        Note_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.NoteTopic_Id, t.Note_Id })
                .ForeignKey("dbo.NoteTopic", t => t.NoteTopic_Id, cascadeDelete: true)
                .ForeignKey("dbo.Note", t => t.Note_Id, cascadeDelete: true)
                .Index(t => t.NoteTopic_Id)
                .Index(t => t.Note_Id);
            
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
                "dbo.RoleGroups",
                c => new
                    {
                        Role_Id = c.Guid(nullable: false),
                        Group_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Group_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        User_Id = c.Guid(nullable: false),
                        Group_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Group_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.BusinessUnitUsers",
                c => new
                    {
                        BusinessUnit_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.BusinessUnit_Id, t.User_Id })
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnit_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.BusinessUnit_Id)
                .Index(t => t.User_Id);
            
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

            CreateViews();
        }
        
        public override void Down()
        {
            DropViews();
            DropSequences();

            DropForeignKey("dbo.SavedEntityMappingField", "SavedEntityMappingId", "dbo.SavedEntityMapping");
            DropForeignKey("dbo.SavedEntityMappingField", "EntityMappingId", "dbo.EntityMapping");
            DropForeignKey("dbo.LedgerAccountMerge", "ToAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.LedgerAccountMerge", "MergedById", "dbo.Users");
            DropForeignKey("dbo.LedgerAccountMerge", "FromAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.LedgerAccountMerge", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.JournalLine", "SourceFundId", "dbo.Fund");
            DropForeignKey("dbo.JournalLine", "SourceBusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.JournalLine", "LedgerAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.JournalLine", "JournalId", "dbo.Journal");
            DropForeignKey("dbo.Journal", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Journal", "ParentJournalId", "dbo.Journal");
            DropForeignKey("dbo.Journal", "BusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.FundFromTo", "ToAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.FundFromTo", "OffsettingFundId", "dbo.Fund");
            DropForeignKey("dbo.Fund", "FundSegmentId", "dbo.Segment");
            DropForeignKey("dbo.FundFromTo", "FundId", "dbo.Fund");
            DropForeignKey("dbo.Fund", "FundBalanceAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.Fund", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Fund", "ClosingRevenueAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.Fund", "ClosingExpenseAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.FundFromTo", "FromAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.FundFromTo", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.AccountClose", "AccountId", "dbo.Account");
            DropForeignKey("dbo.EntityApproval", "ApprovedById", "dbo.Users");
            DropForeignKey("dbo.CustomFieldOption", "CustomFieldId", "dbo.CustomField");
            DropForeignKey("dbo.CustomFieldData", "CustomFieldId", "dbo.CustomField");
            DropForeignKey("dbo.ConstituentPicture", "FileId", "dbo.FileStorage");
            DropForeignKey("dbo.ConstituentPicture", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ChangeSets", "UserId", "dbo.Users");
            DropForeignKey("dbo.PropertyChanges", "ObjectChangeId", "dbo.ObjectChanges");
            DropForeignKey("dbo.ObjectChanges", "ChangeSetId", "dbo.ChangeSets");
            DropForeignKey("dbo.Note", "UserResponsibleId", "dbo.Users");
            DropForeignKey("dbo.Note", "PrimaryContactId", "dbo.Constituent");
            DropForeignKey("dbo.Relationship", "Constituent2Id", "dbo.Constituent");
            DropForeignKey("dbo.Relationship", "Constituent1Id", "dbo.Constituent");
            DropForeignKey("dbo.Relationship", "RelationshipTypeId", "dbo.RelationshipType");
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
            DropForeignKey("dbo.Investment", "RenewalInvestmentType_Id", "dbo.InvestmentType");
            DropForeignKey("dbo.Investment", "InvestmentTypeId", "dbo.InvestmentType");
            DropForeignKey("dbo.InvestmentRelationship", "InvestmentId", "dbo.Investment");
            DropForeignKey("dbo.Investment", "InvestmentOwnershipTypeId", "dbo.InvestmentOwnershipType");
            DropForeignKey("dbo.InvestmentInterestPayout", "InvestmentId", "dbo.Investment");
            DropForeignKey("dbo.InvestmentInterestPayout", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.Investment", "BusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.BusinessUnitUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.BusinessUnitUsers", "BusinessUnit_Id", "dbo.BusinessUnit");
            DropForeignKey("dbo.Users", "DefaultBusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.RoleGroups", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.Users", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.BusinessUnitFromTo", "BusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.BusinessUnitFromTo", "ToAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.BusinessUnitFromTo", "OffsettingBusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.BusinessUnitFromTo", "FromAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.BusinessUnitFromTo", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Ledger", "OrgLedgerId", "dbo.Ledger");
            DropForeignKey("dbo.PostedTransaction", "TransactionId", "dbo.Transaction");
            DropForeignKey("dbo.Transaction", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.EntityTransaction", "TransactionId", "dbo.Transaction");
            DropForeignKey("dbo.Transaction", "DebitAccountId", "dbo.LedgerAccountYear");
            DropForeignKey("dbo.Transaction", "CreditAccountId", "dbo.LedgerAccountYear");
            DropForeignKey("dbo.PostedTransaction", "LedgerAccountYearId", "dbo.LedgerAccountYear");
            DropForeignKey("dbo.PostedTransaction", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.LedgerAccountYear", "LedgerAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.LedgerAccountYear", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.AccountPriorYear", "PriorAccountId", "dbo.Account");
            DropForeignKey("dbo.AccountPriorYear", "AccountId", "dbo.Account");
            DropForeignKey("dbo.LedgerAccountYear", "AccountId", "dbo.Account");
            DropForeignKey("dbo.Account", "Group4Id", "dbo.AccountGroup");
            DropForeignKey("dbo.Account", "Group3Id", "dbo.AccountGroup");
            DropForeignKey("dbo.Account", "Group2Id", "dbo.AccountGroup");
            DropForeignKey("dbo.Account", "Group1Id", "dbo.AccountGroup");
            DropForeignKey("dbo.AccountGroup", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.AccountGroup", "ParentGroupId", "dbo.AccountGroup");
            DropForeignKey("dbo.Account", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Account", "ClosingAccountId", "dbo.Account");
            DropForeignKey("dbo.AccountBudget", "AccountId", "dbo.Account");
            DropForeignKey("dbo.Segment", "SegmentLevelId", "dbo.SegmentLevel");
            DropForeignKey("dbo.SegmentLevel", "LedgerId", "dbo.Ledger");
            DropForeignKey("dbo.Segment", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Segment", "ParentSegmentId", "dbo.Segment");
            DropForeignKey("dbo.AccountSegment", "SegmentId", "dbo.Segment");
            DropForeignKey("dbo.AccountSegment", "AccountId", "dbo.Account");
            DropForeignKey("dbo.AccountBalanceByPeriod", "Id", "dbo.Account");
            DropForeignKey("dbo.LedgerAccount", "LedgerId", "dbo.Ledger");
            DropForeignKey("dbo.FiscalYear", "LedgerId", "dbo.Ledger");
            DropForeignKey("dbo.Ledger", "DefaultFiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Ledger", "BusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.FiscalPeriod", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.InvestmentRelationship", "ConstituentId", "dbo.Constituent");
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
            DropForeignKey("dbo.ContactCategory", "DefaultContactTypeId", "dbo.ContactType");
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
            DropForeignKey("dbo.ConstituentAddress", "AddressTypeId", "dbo.AddressType");
            DropForeignKey("dbo.Address", "AddressType_Id", "dbo.AddressType");
            DropForeignKey("dbo.RegionArea", "RegionId", "dbo.Region");
            DropForeignKey("dbo.Address", "Region4Id", "dbo.Region");
            DropForeignKey("dbo.Address", "Region3Id", "dbo.Region");
            DropForeignKey("dbo.Address", "Region2Id", "dbo.Region");
            DropForeignKey("dbo.Address", "Region1Id", "dbo.Region");
            DropForeignKey("dbo.Region", "ParentRegionId", "dbo.Region");
            DropForeignKey("dbo.ConstituentAddress", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Constituent", "ClergyTypeId", "dbo.ClergyType");
            DropForeignKey("dbo.Constituent", "ClergyStatusId", "dbo.ClergyStatus");
            DropForeignKey("dbo.AlternateId", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.NoteTopicNotes", "Note_Id", "dbo.Note");
            DropForeignKey("dbo.NoteTopicNotes", "NoteTopic_Id", "dbo.NoteTopic");
            DropForeignKey("dbo.Note", "NoteCodeId", "dbo.NoteCode");
            DropForeignKey("dbo.Note", "ContactMethodId", "dbo.NoteContactMethod");
            DropForeignKey("dbo.Note", "CategoryId", "dbo.NoteCategory");
            DropForeignKey("dbo.Attachment", "NoteId", "dbo.Note");
            DropForeignKey("dbo.Attachment", "FileId", "dbo.FileStorage");
            DropIndex("dbo.PaymentMethodConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.PaymentMethodConstituents", new[] { "PaymentMethod_Id" });
            DropIndex("dbo.BusinessUnitUsers", new[] { "User_Id" });
            DropIndex("dbo.BusinessUnitUsers", new[] { "BusinessUnit_Id" });
            DropIndex("dbo.UserGroups", new[] { "Group_Id" });
            DropIndex("dbo.UserGroups", new[] { "User_Id" });
            DropIndex("dbo.RoleGroups", new[] { "Group_Id" });
            DropIndex("dbo.RoleGroups", new[] { "Role_Id" });
            DropIndex("dbo.EthnicityConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.EthnicityConstituents", new[] { "Ethnicity_Id" });
            DropIndex("dbo.DenominationConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.DenominationConstituents", new[] { "Denomination_Id" });
            DropIndex("dbo.TagConstituentTypes", new[] { "ConstituentType_Id" });
            DropIndex("dbo.TagConstituentTypes", new[] { "Tag_Id" });
            DropIndex("dbo.TagConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.TagConstituents", new[] { "Tag_Id" });
            DropIndex("dbo.NoteTopicNotes", new[] { "Note_Id" });
            DropIndex("dbo.NoteTopicNotes", new[] { "NoteTopic_Id" });
            DropIndex("dbo.SavedEntityMappingField", new[] { "EntityMappingId" });
            DropIndex("dbo.SavedEntityMappingField", new[] { "SavedEntityMappingId" });
            DropIndex("dbo.SavedEntityMapping", new[] { "Name" });
            DropIndex("dbo.LedgerAccountMerge", new[] { "MergedById" });
            DropIndex("dbo.LedgerAccountMerge", new[] { "FiscalYearId" });
            DropIndex("dbo.LedgerAccountMerge", new[] { "ToAccountId" });
            DropIndex("dbo.LedgerAccountMerge", new[] { "FromAccountId" });
            DropIndex("dbo.Journal", new[] { "ParentJournalId" });
            DropIndex("dbo.Journal", new[] { "FiscalYearId" });
            DropIndex("dbo.Journal", new[] { "BusinessUnitId" });
            DropIndex("dbo.JournalLine", new[] { "JournalId" });
            DropIndex("dbo.JournalLine", new[] { "SourceFundId" });
            DropIndex("dbo.JournalLine", new[] { "SourceBusinessUnitId" });
            DropIndex("dbo.JournalLine", new[] { "LedgerAccountId" });
            DropIndex("dbo.Fund", new[] { "ClosingExpenseAccountId" });
            DropIndex("dbo.Fund", new[] { "ClosingRevenueAccountId" });
            DropIndex("dbo.Fund", new[] { "FundBalanceAccountId" });
            DropIndex("dbo.Fund", "IX_FiscalYear_FundSegment");
            DropIndex("dbo.FundFromTo", new[] { "ToAccountId" });
            DropIndex("dbo.FundFromTo", new[] { "FromAccountId" });
            DropIndex("dbo.FundFromTo", "IX_FiscalYear_Funds");
            DropIndex("dbo.AccountClose", new[] { "AccountId" });
            DropIndex("dbo.EntityApproval", new[] { "ApprovedById" });
            DropIndex("dbo.EntityApproval", new[] { "EntityType" });
            DropIndex("dbo.EntityApproval", new[] { "ParentEntityId" });
            DropIndex("dbo.CustomFieldOption", new[] { "CustomFieldId" });
            DropIndex("dbo.CustomFieldData", new[] { "CustomFieldId" });
            DropIndex("dbo.ConstituentPicture", new[] { "FileId" });
            DropIndex("dbo.ConstituentPicture", new[] { "ConstituentId" });
            DropIndex("dbo.PropertyChanges", new[] { "ObjectChangeId" });
            DropIndex("dbo.ObjectChanges", new[] { "ChangeSetId" });
            DropIndex("dbo.ChangeSets", new[] { "UserId" });
            DropIndex("dbo.RelationshipType", new[] { "RelationshipCategoryId" });
            DropIndex("dbo.RelationshipType", new[] { "ReciprocalTypeFemaleId" });
            DropIndex("dbo.RelationshipType", new[] { "ReciprocalTypeMaleId" });
            DropIndex("dbo.RelationshipType", new[] { "Name" });
            DropIndex("dbo.RelationshipType", new[] { "Code" });
            DropIndex("dbo.Relationship", new[] { "Constituent2Id" });
            DropIndex("dbo.Relationship", new[] { "Constituent1Id" });
            DropIndex("dbo.Relationship", new[] { "RelationshipTypeId" });
            DropIndex("dbo.Profession", new[] { "Code" });
            DropIndex("dbo.Profession", new[] { "Name" });
            DropIndex("dbo.Prefix", new[] { "GenderId" });
            DropIndex("dbo.Prefix", new[] { "Name" });
            DropIndex("dbo.Prefix", new[] { "Code" });
            DropIndex("dbo.EFTFormat", new[] { "Name" });
            DropIndex("dbo.EFTFormat", new[] { "Code" });
            DropIndex("dbo.PaymentMethod", new[] { "EFTFormatId" });
            DropIndex("dbo.MaritialStatus", new[] { "Name" });
            DropIndex("dbo.MaritialStatus", new[] { "Code" });
            DropIndex("dbo.Language", new[] { "Name" });
            DropIndex("dbo.Language", new[] { "Code" });
            DropIndex("dbo.InvestmentType", new[] { "Name" });
            DropIndex("dbo.InvestmentType", new[] { "Code" });
            DropIndex("dbo.InvestmentOwnershipType", new[] { "Name" });
            DropIndex("dbo.InvestmentOwnershipType", new[] { "Code" });
            DropIndex("dbo.InvestmentInterestPayout", new[] { "ConstituentId" });
            DropIndex("dbo.InvestmentInterestPayout", new[] { "InvestmentId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "ConstituentId" });
            DropIndex("dbo.Users", new[] { "DefaultBusinessUnitId" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.EntityTransaction", new[] { "TransactionId" });
            DropIndex("dbo.EntityTransaction", new[] { "EntityType" });
            DropIndex("dbo.EntityTransaction", new[] { "ParentEntityId" });
            DropIndex("dbo.Transaction", new[] { "CreditAccountId" });
            DropIndex("dbo.Transaction", new[] { "DebitAccountId" });
            DropIndex("dbo.Transaction", new[] { "FiscalYearId" });
            DropIndex("dbo.PostedTransaction", new[] { "TransactionId" });
            DropIndex("dbo.PostedTransaction", new[] { "FiscalYearId" });
            DropIndex("dbo.PostedTransaction", new[] { "LedgerAccountYearId" });
            DropIndex("dbo.AccountPriorYear", new[] { "PriorAccountId" });
            DropIndex("dbo.AccountPriorYear", new[] { "AccountId" });
            DropIndex("dbo.AccountGroup", new[] { "ParentGroupId" });
            DropIndex("dbo.AccountGroup", new[] { "FiscalYearId" });
            DropIndex("dbo.AccountBudget", "IX_Account_BudgetType");
            DropIndex("dbo.SegmentLevel", "IX_Name");
            DropIndex("dbo.SegmentLevel", "IX_Level");
            DropIndex("dbo.Segment", new[] { "ParentSegmentId" });
            DropIndex("dbo.Segment", new[] { "SegmentLevelId" });
            DropIndex("dbo.Segment", new[] { "FiscalYearId" });
            DropIndex("dbo.AccountSegment", new[] { "SegmentId" });
            DropIndex("dbo.AccountSegment", new[] { "AccountId" });
            DropIndex("dbo.AccountBalanceByPeriod", new[] { "Id" });
            DropIndex("dbo.Account", new[] { "Group4Id" });
            DropIndex("dbo.Account", new[] { "Group3Id" });
            DropIndex("dbo.Account", new[] { "Group2Id" });
            DropIndex("dbo.Account", new[] { "Group1Id" });
            DropIndex("dbo.Account", new[] { "ClosingAccountId" });
            DropIndex("dbo.Account", new[] { "SortKey" });
            DropIndex("dbo.Account", new[] { "FiscalYearId" });
            DropIndex("dbo.LedgerAccountYear", new[] { "AccountId" });
            DropIndex("dbo.LedgerAccountYear", new[] { "FiscalYearId" });
            DropIndex("dbo.LedgerAccountYear", new[] { "LedgerAccountId" });
            DropIndex("dbo.LedgerAccount", new[] { "LedgerId" });
            DropIndex("dbo.Ledger", new[] { "OrgLedgerId" });
            DropIndex("dbo.Ledger", new[] { "BusinessUnitId" });
            DropIndex("dbo.Ledger", new[] { "Name" });
            DropIndex("dbo.Ledger", new[] { "Code" });
            DropIndex("dbo.Ledger", new[] { "DefaultFiscalYearId" });
            DropIndex("dbo.FiscalPeriod", "IX_FiscalYear_PeriodNumber");
            DropIndex("dbo.FiscalYear", "IX_Name");
            DropIndex("dbo.BusinessUnitFromTo", new[] { "ToAccountId" });
            DropIndex("dbo.BusinessUnitFromTo", new[] { "FromAccountId" });
            DropIndex("dbo.BusinessUnitFromTo", "IX_FiscalYear_BUs");
            DropIndex("dbo.BusinessUnit", new[] { "Code" });
            DropIndex("dbo.BusinessUnit", new[] { "Name" });
            DropIndex("dbo.Investment", new[] { "RenewalInvestmentType_Id" });
            DropIndex("dbo.Investment", new[] { "InvestmentTypeId" });
            DropIndex("dbo.Investment", new[] { "InvestmentOwnershipTypeId" });
            DropIndex("dbo.Investment", new[] { "BusinessUnitId" });
            DropIndex("dbo.InvestmentRelationship", new[] { "InvestmentId" });
            DropIndex("dbo.InvestmentRelationship", new[] { "ConstituentId" });
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
            DropIndex("dbo.Education", new[] { "ConstituentId" });
            DropIndex("dbo.Education", new[] { "DegreeId" });
            DropIndex("dbo.Education", new[] { "SchoolId" });
            DropIndex("dbo.EducationLevel", new[] { "Name" });
            DropIndex("dbo.EducationLevel", new[] { "Code" });
            DropIndex("dbo.DoingBusinessAs", new[] { "ConstituentId" });
            DropIndex("dbo.Denomination", new[] { "Name" });
            DropIndex("dbo.Denomination", new[] { "Code" });
            DropIndex("dbo.ContactCategory", new[] { "DefaultContactTypeId" });
            DropIndex("dbo.ContactCategory", new[] { "Name" });
            DropIndex("dbo.ContactCategory", new[] { "Code" });
            DropIndex("dbo.ContactType", new[] { "Name" });
            DropIndex("dbo.ContactType", "IX_Code");
            DropIndex("dbo.ContactInfo", new[] { "ParentContactId" });
            DropIndex("dbo.ContactInfo", new[] { "ContactTypeId" });
            DropIndex("dbo.ContactInfo", new[] { "ConstituentId" });
            DropIndex("dbo.Tag", new[] { "TagGroupId" });
            DropIndex("dbo.Tag", new[] { "Name" });
            DropIndex("dbo.Tag", new[] { "Code" });
            DropIndex("dbo.ConstituentType", new[] { "Name" });
            DropIndex("dbo.ConstituentType", new[] { "Code" });
            DropIndex("dbo.ConstituentStatus", new[] { "Name" });
            DropIndex("dbo.ConstituentStatus", new[] { "Code" });
            DropIndex("dbo.AddressType", new[] { "Name" });
            DropIndex("dbo.AddressType", new[] { "Code" });
            DropIndex("dbo.RegionArea", new[] { "RegionId" });
            DropIndex("dbo.Region", new[] { "ParentRegionId" });
            DropIndex("dbo.Region", new[] { "Name" });
            DropIndex("dbo.Region", new[] { "Code" });
            DropIndex("dbo.Address", new[] { "AddressType_Id" });
            DropIndex("dbo.Address", new[] { "Region4Id" });
            DropIndex("dbo.Address", new[] { "Region3Id" });
            DropIndex("dbo.Address", new[] { "Region2Id" });
            DropIndex("dbo.Address", new[] { "Region1Id" });
            DropIndex("dbo.ConstituentAddress", new[] { "DuplicateKey" });
            DropIndex("dbo.ConstituentAddress", new[] { "AddressTypeId" });
            DropIndex("dbo.ConstituentAddress", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentAddress", new[] { "AddressId" });
            DropIndex("dbo.ClergyType", new[] { "Name" });
            DropIndex("dbo.ClergyType", new[] { "Code" });
            DropIndex("dbo.ClergyStatus", new[] { "Name" });
            DropIndex("dbo.ClergyStatus", new[] { "Code" });
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
            DropIndex("dbo.NoteTopic", new[] { "Name" });
            DropIndex("dbo.NoteTopic", new[] { "Code" });
            DropIndex("dbo.NoteCode", new[] { "Name" });
            DropIndex("dbo.NoteCode", new[] { "Code" });
            DropIndex("dbo.NoteContactMethod", new[] { "Name" });
            DropIndex("dbo.NoteContactMethod", new[] { "Code" });
            DropIndex("dbo.Note", new[] { "EntityType" });
            DropIndex("dbo.Note", new[] { "ParentEntityId" });
            DropIndex("dbo.Note", new[] { "UserResponsibleId" });
            DropIndex("dbo.Note", new[] { "ContactMethodId" });
            DropIndex("dbo.Note", new[] { "PrimaryContactId" });
            DropIndex("dbo.Note", new[] { "NoteCodeId" });
            DropIndex("dbo.Note", new[] { "CategoryId" });
            DropIndex("dbo.Attachment", new[] { "EntityType" });
            DropIndex("dbo.Attachment", new[] { "ParentEntityId" });
            DropIndex("dbo.Attachment", new[] { "NoteId" });
            DropIndex("dbo.Attachment", new[] { "FileId" });
            DropTable("dbo.PaymentMethodConstituents");
            DropTable("dbo.BusinessUnitUsers");
            DropTable("dbo.UserGroups");
            DropTable("dbo.RoleGroups");
            DropTable("dbo.EthnicityConstituents");
            DropTable("dbo.DenominationConstituents");
            DropTable("dbo.TagConstituentTypes");
            DropTable("dbo.TagConstituents");
            DropTable("dbo.NoteTopicNotes");
            DropTable("dbo.TransactionXref");
            DropTable("dbo.SectionPreference");
            DropTable("dbo.SavedEntityMappingField");
            DropTable("dbo.SavedEntityMapping");
            DropTable("dbo.LedgerAccountMerge");
            DropTable("dbo.Journal");
            DropTable("dbo.JournalLine");
            DropTable("dbo.GLAccountSelection");
            DropTable("dbo.Fund");
            DropTable("dbo.FundFromTo");
            DropTable("dbo.AccountClose");
            DropTable("dbo.EntityNumber");
            DropTable("dbo.EntityMapping");
            DropTable("dbo.EntityApproval");
            DropTable("dbo.CustomFieldOption");
            DropTable("dbo.CustomFieldData");
            DropTable("dbo.CustomField");
            DropTable("dbo.RegionLevel");
            DropTable("dbo.ConstituentPicture");
            DropTable("dbo.Configuration");
            DropTable("dbo.PropertyChanges");
            DropTable("dbo.ObjectChanges");
            DropTable("dbo.ChangeSets");
            DropTable("dbo.RelationshipCategory");
            DropTable("dbo.RelationshipType");
            DropTable("dbo.Relationship");
            DropTable("dbo.Profession");
            DropTable("dbo.Prefix");
            DropTable("dbo.EFTFormat");
            DropTable("dbo.PaymentMethod");
            DropTable("dbo.MaritialStatus");
            DropTable("dbo.Language");
            DropTable("dbo.InvestmentType");
            DropTable("dbo.InvestmentOwnershipType");
            DropTable("dbo.InvestmentInterestPayout");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.Groups");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.EntityTransaction");
            DropTable("dbo.Transaction");
            DropTable("dbo.PostedTransaction");
            DropTable("dbo.AccountPriorYear");
            DropTable("dbo.AccountGroup");
            DropTable("dbo.AccountBudget");
            DropTable("dbo.SegmentLevel");
            DropTable("dbo.Segment");
            DropTable("dbo.AccountSegment");
            DropTable("dbo.AccountBalanceByPeriod");
            DropTable("dbo.Account");
            DropTable("dbo.LedgerAccountYear");
            DropTable("dbo.LedgerAccount");
            DropTable("dbo.Ledger");
            DropTable("dbo.FiscalPeriod");
            DropTable("dbo.FiscalYear");
            DropTable("dbo.BusinessUnitFromTo");
            DropTable("dbo.BusinessUnit");
            DropTable("dbo.Investment");
            DropTable("dbo.InvestmentRelationship");
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
            DropTable("dbo.AddressType");
            DropTable("dbo.RegionArea");
            DropTable("dbo.Region");
            DropTable("dbo.Address");
            DropTable("dbo.ConstituentAddress");
            DropTable("dbo.ClergyType");
            DropTable("dbo.ClergyStatus");
            DropTable("dbo.AlternateId");
            DropTable("dbo.Constituent");
            DropTable("dbo.NoteTopic");
            DropTable("dbo.NoteCode");
            DropTable("dbo.NoteContactMethod");
            DropTable("dbo.NoteCategory");
            DropTable("dbo.Note");
            DropTable("dbo.FileStorage");
            DropTable("dbo.Attachment");
        }

        private string _uniqueFileGroupName;

        private void CreateFileGroup()
        {
            _uniqueFileGroupName = "FileGroup_" + DateTime.Now.Ticks.ToString();

            Sql(@"
                    IF NOT EXISTS (SELECT * FROM sys.filegroups where name = '" + _uniqueFileGroupName + @"') BEGIN
                        ALTER DATABASE CURRENT
                            ADD FILEGROUP [" + _uniqueFileGroupName + @"] contains filestream
                    END
                ", true);

            Sql(@"
                IF EXISTS (SELECT * FROM sys.filegroups where name = '" + _uniqueFileGroupName + @"') AND NOT EXISTS (SELECT * FROM sys.master_files where name = DB_NAME() + '_" + _uniqueFileGroupName + @"') BEGIN
                    DECLARE @DatabasePath nvarchar(max)
                    DECLARE @SQL nvarchar(max)
                    
                    SELECT @DatabasePath = 'F:\" + _uniqueFileGroupName + @".ndf'
                    
                    SET @SQL = N'ALTER DATABASE CURRENT
                                    ADD FILE (
                                        NAME = [' + DB_NAME() + '" + _uniqueFileGroupName + @"],
                                        FILENAME =  N''' + @DatabasePath + ''', 
                                        MAXSIZE = UNLIMITED
                                        )
                                    TO FILEGROUP [" + _uniqueFileGroupName + @"]'
                    EXECUTE sp_executesql @SQL
                END
            ", true);
        }

        private void CreateFileStorageTable()
        {

            Sql(@"
                    CREATE TABLE [dbo].[FileStorage](
	                    [Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	                    [Name] [varchar](256) NOT NULL,
	                    [Extension] [varchar](8) NOT NULL,
	                    [Size] [bigint] NOT NULL,
	                    [Data] [varbinary](max) FILESTREAM  NULL,
	                    [CreatedBy] [varchar](64) NULL,
	                    [CreatedOn] [datetime] NULL,
                     CONSTRAINT [PK_FileStorage] PRIMARY KEY CLUSTERED 
                    (
	                    [Id] ASC
                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] FILESTREAM_ON [Demo1_Filestream]
                    ) ON [PRIMARY] FILESTREAM_ON [" + _uniqueFileGroupName + @"]
                    ");
        }

        private void CreateViews()
        {

            this.CreateView("AccountBalanceByPeriod",
                "WITH T AS (SELECT a.Id, pt.PeriodNumber, CASE WHEN pt.Amount < 0 THEN 'CR' ELSE 'DB' END AS DebitCredit, pt.Amount " +
                "FROM PostedTransaction AS pt INNER JOIN " +
                "LedgerAccountYear AS lay ON lay.Id = pt.LedgerAccountYearId INNER JOIN " +
                "Account AS a ON a.Id = lay.AccountId " +
                "WHERE (pt.PostedTransactionType = 0)) " +
                "SELECT Id, PeriodNumber, DebitCredit, ABS(SUM(Amount)) AS TotalAmount " +
                "FROM T AS T_1 " +
                "GROUP BY Id, PeriodNumber, DebitCredit");

            this.CreateView("GLAccountSelection",
                "select l.AccountGroup1Title + ': ' + ag1.Name as Level1, l.AccountGroup2Title + ': ' + ag2.Name as Level2, " +
                "l.AccountGroup3Title + ': ' + ag3.Name as Level3, l.AccountGroup4Title + ': ' + ag4.Name as Level4, " +
                "acct.AccountNumber, acct.Name as Description,	acct.Id, l.Id as LedgerId, ag1.Sequence as LevelSequence1, ag2.Sequence as LevelSequence2, " +
                "Ag3.Sequence as LevelSequence3, Ag4.Sequence as LevelSequence4, " +
                "fy.Id as FiscalYearId, lay.LedgerAccountId, acct.SortKey as SortKey " +
                "FROM Account acct " +
                "LEFT Outer join AccountGroup ag1 on ag1.Id = acct.Group1Id " +
                "left outer join AccountGroup ag2 on ag2.Id = acct.Group2Id " +
                "left outer join AccountGroup ag3 on ag3.Id = acct.Group3Id " +
                "left outer join AccountGroup ag4 on ag4.Id = acct.Group4Id " +
                "left outer join FiscalYear fy on fy.Id = acct.FiscalYearId " +
                "inner join Ledger l on fy.LedgerId = l.Id " +
                "JOIN	(SELECT		MIN(LedgerAccountId) AS LedgerAccountId, AccountId " +
                "FROM        LedgerAccountYear lay1 " +
                "GROUP BY lay1.AccountId) lay " +
                "ON  lay.AccountId = acct.Id" +
                "group by ag1.Sequence, ag2.Sequence, Ag3.Sequence, Ag4.Sequence, " +
                "l.AccountGroup1Title + ': ' + ag1.Name, " +
                "l.AccountGroup2Title + ': ' + ag2.Name, " +
                "l.AccountGroup3Title + ': ' + ag3.Name, " +
                "l.AccountGroup4Title + ': ' + ag4.Name, " +
                "lay.LedgerAccountId, " +
                "acct.SortKey, " +
                "acct.AccountNumber, " +
                "acct.Id, " +
                "l.Id, " +
                "fy.Id, " +
                "acct.Name "
                );

        }

        private void CreateSequences()
        {
            this.CreateSequence(Sequences.TransactionNumber, "bigint");
        }

        private void DropViews()
        {
            this.DropView("GLAccountSelection");
            this.DropView("AccountBalanceByPeriod");
        }

        private void DropSequences()
        {
            this.DropSequence(Sequences.TransactionNumber);
        }


    }
}

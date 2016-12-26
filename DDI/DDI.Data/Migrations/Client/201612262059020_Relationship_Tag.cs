namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relationship_Tag : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        TagGroupId = c.Guid(),
                        Order = c.Int(nullable: false),
                        ConstituentCategory = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TagGroup", t => t.TagGroupId)
                .Index(t => t.TagGroupId);
            
            CreateTable(
                "dbo.TagGroup",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Order = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        TagSelectionType = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactCategory",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(maxLength: 1),
                        Name = c.String(maxLength: 128),
                        SectionTitle = c.String(maxLength: 128),
                        TextBoxLabel = c.String(maxLength: 128),
                        DefaultContactTypeID = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactType", t => t.DefaultContactTypeID)
                .Index(t => t.DefaultContactTypeID);
            
            CreateTable(
                "dbo.Degree",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.School",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Relationsihp",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RelationshipTypeId = c.Guid(),
                        Constituent1Id = c.Guid(),
                        Constituent2Id = c.Guid(),
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
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        ReciprocalTypeMaleId = c.Guid(),
                        ReciprocalTypeFemaleId = c.Guid(),
                        IsSpouse = c.Boolean(nullable: false),
                        ConstituentCategory = c.Int(nullable: false),
                        RelationshipCategoryId = c.Guid(),
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
                        Id = c.Guid(nullable: false, identity: true),
                        Code = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        IsShownInQuickView = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RegionArea",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        RegionId = c.Guid(),
                        CountryId = c.Guid(),
                        StateId = c.Guid(),
                        CountyId = c.Guid(),
                        City = c.String(maxLength: 128),
                        PostalCodeLow = c.String(maxLength: 128),
                        PostalCodeHigh = c.String(maxLength: 128),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Region", t => t.RegionId)
                .Index(t => t.RegionId);
            
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
            
            AddColumn("dbo.ConstituentType", "NameFormat", c => c.String(maxLength: 128));
            AddColumn("dbo.ConstituentType", "SalutationFormal", c => c.String(maxLength: 128));
            AddColumn("dbo.ConstituentType", "SalutationInformal", c => c.String(maxLength: 128));
            RenameColumn("dbo.ContactType", "Description", "Name");
            AddColumn("dbo.ContactType", "ContactCategoryId", c => c.Guid());
            AddColumn("dbo.ContactType", "IsAlwaysShown", c => c.Boolean(nullable: false));
            AddColumn("dbo.ContactType", "CanDelete", c => c.Boolean(nullable: false));
            AddColumn("dbo.Education", "SchoolId", c => c.Guid());
            AddColumn("dbo.Education", "DegreeId", c => c.Guid());
            CreateIndex("dbo.ContactType", "ContactCategoryId");
            CreateIndex("dbo.Education", "SchoolId");
            CreateIndex("dbo.Education", "DegreeId");
            AddForeignKey("dbo.ContactType", "ContactCategoryId", "dbo.ContactCategory", "Id");
            AddForeignKey("dbo.Education", "DegreeId", "dbo.Degree", "Id");
            AddForeignKey("dbo.Education", "SchoolId", "dbo.School", "Id");
            DropColumn("dbo.Education", "School");
            DropColumn("dbo.Education", "SchoolCode");
            DropColumn("dbo.Education", "DegreeCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Education", "DegreeCode", c => c.String(maxLength: 128));
            AddColumn("dbo.Education", "SchoolCode", c => c.String(maxLength: 128));
            AddColumn("dbo.Education", "School", c => c.String(maxLength: 128));
            AddColumn("dbo.ContactType", "Description", c => c.String(maxLength: 128));
            DropForeignKey("dbo.RegionArea", "RegionId", "dbo.Region");
            DropForeignKey("dbo.Relationsihp", "Constituent2Id", "dbo.Constituent");
            DropForeignKey("dbo.Relationsihp", "Constituent1Id", "dbo.Constituent");
            DropForeignKey("dbo.Relationsihp", "RelationshipTypeId", "dbo.RelationshipType");
            DropForeignKey("dbo.RelationshipType", "RelationshipCategoryId", "dbo.RelationshipCategory");
            DropForeignKey("dbo.RelationshipType", "ReciprocalTypeMaleId", "dbo.RelationshipType");
            DropForeignKey("dbo.RelationshipType", "ReciprocalTypeFemaleId", "dbo.RelationshipType");
            DropForeignKey("dbo.Education", "SchoolId", "dbo.School");
            DropForeignKey("dbo.Education", "DegreeId", "dbo.Degree");
            DropForeignKey("dbo.ContactCategory", "DefaultContactTypeID", "dbo.ContactType");
            DropForeignKey("dbo.ContactType", "ContactCategoryId", "dbo.ContactCategory");
            DropForeignKey("dbo.Tag", "TagGroupId", "dbo.TagGroup");
            DropForeignKey("dbo.TagConstituentTypes", "ConstituentType_Id", "dbo.ConstituentType");
            DropForeignKey("dbo.TagConstituentTypes", "Tag_Id", "dbo.Tag");
            DropForeignKey("dbo.TagConstituents", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.TagConstituents", "Tag_Id", "dbo.Tag");
            DropIndex("dbo.TagConstituentTypes", new[] { "ConstituentType_Id" });
            DropIndex("dbo.TagConstituentTypes", new[] { "Tag_Id" });
            DropIndex("dbo.TagConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.TagConstituents", new[] { "Tag_Id" });
            DropIndex("dbo.RegionArea", new[] { "RegionId" });
            DropIndex("dbo.RelationshipType", new[] { "RelationshipCategoryId" });
            DropIndex("dbo.RelationshipType", new[] { "ReciprocalTypeFemaleId" });
            DropIndex("dbo.RelationshipType", new[] { "ReciprocalTypeMaleId" });
            DropIndex("dbo.Relationsihp", new[] { "Constituent2Id" });
            DropIndex("dbo.Relationsihp", new[] { "Constituent1Id" });
            DropIndex("dbo.Relationsihp", new[] { "RelationshipTypeId" });
            DropIndex("dbo.Education", new[] { "DegreeId" });
            DropIndex("dbo.Education", new[] { "SchoolId" });
            DropIndex("dbo.ContactCategory", new[] { "DefaultContactTypeID" });
            DropIndex("dbo.ContactType", new[] { "ContactCategoryId" });
            DropIndex("dbo.Tag", new[] { "TagGroupId" });
            DropColumn("dbo.Education", "DegreeId");
            DropColumn("dbo.Education", "SchoolId");
            DropColumn("dbo.ContactType", "CanDelete");
            DropColumn("dbo.ContactType", "IsAlwaysShown");
            DropColumn("dbo.ContactType", "ContactCategoryId");
            RenameColumn("dbo.ContactType", "Name", "Description");
            DropColumn("dbo.ConstituentType", "SalutationInformal");
            DropColumn("dbo.ConstituentType", "SalutationFormal");
            DropColumn("dbo.ConstituentType", "NameFormat");
            DropTable("dbo.TagConstituentTypes");
            DropTable("dbo.TagConstituents");
            DropTable("dbo.RegionArea");
            DropTable("dbo.RelationshipCategory");
            DropTable("dbo.RelationshipType");
            DropTable("dbo.Relationsihp");
            DropTable("dbo.School");
            DropTable("dbo.Degree");
            DropTable("dbo.ContactCategory");
            DropTable("dbo.TagGroup");
            DropTable("dbo.Tag");
        }
    }
}

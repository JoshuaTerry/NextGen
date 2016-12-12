namespace DDI.Data.Migrations.Common
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abbreviation",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Word = c.String(maxLength: 32),
                        USPSAbbreviation = c.String(maxLength: 32),
                        AddressWord = c.String(maxLength: 32),
                        NameWord = c.String(maxLength: 32),
                        IsSuffix = c.Boolean(nullable: false),
                        IsCaps = c.Boolean(nullable: false),
                        IsSecondary = c.Boolean(nullable: false),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        PlaceCode = c.String(maxLength: 8),
                        StateId = c.Guid(),
                        CountyId = c.Guid(),
                        Population = c.Int(nullable: false),
                        PopulationPercentageChange = c.Decimal(nullable: false, storeType: "money"),
                        CoordinateNS = c.Decimal(nullable: false, storeType: "money"),
                        CoordinateEW = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.County", t => t.CountyId)
                .ForeignKey("dbo.State", t => t.StateId)
                .Index(t => t.StateId)
                .Index(t => t.CountyId);
            
            CreateTable(
                "dbo.CityName",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Description = c.String(),
                        IsPreferred = c.Boolean(nullable: false),
                        CityId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.City", t => t.CityId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.County",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Description = c.String(),
                        FIPSCode = c.String(maxLength: 5),
                        LegacyCode = c.String(maxLength: 4),
                        StateId = c.Guid(),
                        Population = c.Int(nullable: false),
                        PopulationPerSqaureMile = c.Decimal(nullable: false, storeType: "money"),
                        PopulationPercentageChange = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.State", t => t.StateId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        StateCode = c.String(maxLength: 4),
                        Description = c.String(),
                        FIPSCode = c.String(maxLength: 2),
                        CountryId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CountryCode = c.String(maxLength: 4),
                        Description = c.String(),
                        ISOCode = c.String(maxLength: 2),
                        LegacyCode = c.String(maxLength: 4),
                        StateName = c.String(),
                        StateAbbreviation = c.String(maxLength: 4),
                        PostalCodeFormat = c.String(),
                        AddressFormat = c.String(),
                        CallingCode = c.String(maxLength: 4),
                        InternationalPrefix = c.String(maxLength: 4),
                        TrunkPrefix = c.String(maxLength: 4),
                        PhoneFormat = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Zip",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ZipCode = c.String(maxLength: 5),
                        CoordinateNS = c.Decimal(nullable: false, storeType: "money"),
                        CoordinateEW = c.Decimal(nullable: false, storeType: "money"),
                        CityId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.City", t => t.CityId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.ZipBranch",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Description = c.String(),
                        USPSKey = c.String(maxLength: 8),
                        FacilityCode = c.String(maxLength: 2),
                        IsPreferred = c.Boolean(nullable: false),
                        ZipId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Zip", t => t.ZipId)
                .Index(t => t.ZipId);
            
            CreateTable(
                "dbo.ZipStreet",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Prefix = c.String(maxLength: 8),
                        Street = c.String(),
                        Suffix = c.String(maxLength: 8),
                        Suffix2 = c.String(maxLength: 8),
                        UrbanizationKey = c.String(maxLength: 8),
                        CityKey = c.String(maxLength: 8),
                        ZipId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Zip", t => t.ZipId)
                .Index(t => t.ZipId);
            
            CreateTable(
                "dbo.ZipPlus4",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UpdateKey = c.String(maxLength: 16),
                        AddressLow = c.String(maxLength: 16),
                        AddressHigh = c.String(maxLength: 16),
                        SecondaryAbbrev = c.String(maxLength: 8),
                        SecondaryLow = c.String(maxLength: 16),
                        SecondaryHigh = c.String(maxLength: 16),
                        Plus4 = c.String(maxLength: 4),
                        AddressType = c.Int(nullable: false),
                        SecondaryType = c.Int(nullable: false),
                        IsRange = c.Boolean(nullable: false),
                        ZipStreetId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ZipStreet", t => t.ZipStreetId)
                .Index(t => t.ZipStreetId);
            
            CreateTable(
                "dbo.Thesaurus",
                c => new
                    {
                        Word = c.String(nullable: false, maxLength: 50),
                        Expansion = c.String(),
                    })
                .PrimaryKey(t => t.Word);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZipPlus4", "ZipStreetId", "dbo.ZipStreet");
            DropForeignKey("dbo.ZipStreet", "ZipId", "dbo.Zip");
            DropForeignKey("dbo.ZipBranch", "ZipId", "dbo.Zip");
            DropForeignKey("dbo.Zip", "CityId", "dbo.City");
            DropForeignKey("dbo.State", "CountryId", "dbo.Country");
            DropForeignKey("dbo.County", "StateId", "dbo.State");
            DropForeignKey("dbo.City", "StateId", "dbo.State");
            DropForeignKey("dbo.City", "CountyId", "dbo.County");
            DropForeignKey("dbo.CityName", "CityId", "dbo.City");
            DropIndex("dbo.ZipPlus4", new[] { "ZipStreetId" });
            DropIndex("dbo.ZipStreet", new[] { "ZipId" });
            DropIndex("dbo.ZipBranch", new[] { "ZipId" });
            DropIndex("dbo.Zip", new[] { "CityId" });
            DropIndex("dbo.State", new[] { "CountryId" });
            DropIndex("dbo.County", new[] { "StateId" });
            DropIndex("dbo.CityName", new[] { "CityId" });
            DropIndex("dbo.City", new[] { "CountyId" });
            DropIndex("dbo.City", new[] { "StateId" });
            DropTable("dbo.Thesaurus");
            DropTable("dbo.ZipPlus4");
            DropTable("dbo.ZipStreet");
            DropTable("dbo.ZipBranch");
            DropTable("dbo.Zip");
            DropTable("dbo.Country");
            DropTable("dbo.State");
            DropTable("dbo.County");
            DropTable("dbo.CityName");
            DropTable("dbo.City");
            DropTable("dbo.Abbreviation");
        }
    }
}

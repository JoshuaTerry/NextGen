namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Constituent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CityName", "CityId", "dbo.City");
            DropForeignKey("dbo.City", "CountyId", "dbo.County");
            DropForeignKey("dbo.County", "StateId", "dbo.State");
            DropForeignKey("dbo.City", "StateId", "dbo.State");
            DropForeignKey("dbo.Zip", "CityId", "dbo.City");
            DropForeignKey("dbo.ZipBranch", "ZipId", "dbo.Zip");
            DropForeignKey("dbo.ZipStreet", "ZipId", "dbo.Zip");
            DropForeignKey("dbo.ZipPlus4", "ZipStreetId", "dbo.ZipStreet");
            DropForeignKey("dbo.State", "CountryId", "dbo.Country");
            DropForeignKey("dbo.EducationLevel", "Education_Id", "dbo.Education");
            DropForeignKey("dbo.EducationLevel", "EducationId", "dbo.Education");
            DropIndex("dbo.State", new[] { "CountryId" });
            DropIndex("dbo.City", new[] { "StateId" });
            DropIndex("dbo.City", new[] { "CountyId" });
            DropIndex("dbo.CityName", new[] { "CityId" });
            DropIndex("dbo.County", new[] { "StateId" });
            DropIndex("dbo.Zip", new[] { "CityId" });
            DropIndex("dbo.ZipBranch", new[] { "ZipId" });
            DropIndex("dbo.ZipStreet", new[] { "ZipId" });
            DropIndex("dbo.ZipPlus4", new[] { "ZipStreetId" });
            DropIndex("dbo.EducationLevel", new[] { "EducationId" });
            DropIndex("dbo.EducationLevel", new[] { "Education_Id" });
            RenameColumn(table: "dbo.PaymentPreference", name: "ConstituentId", newName: "Constituent_Id");
            RenameIndex(table: "dbo.PaymentPreference", name: "IX_ConstituentId", newName: "IX_Constituent_Id");
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
            
            AddColumn("dbo.State", "Abbreviation", c => c.String());
            AddColumn("dbo.State", "Name", c => c.String());
            AddColumn("dbo.Prefix", "Descriptin", c => c.String());
            DropColumn("dbo.State", "StateCode");
            DropColumn("dbo.State", "Description");
            DropColumn("dbo.State", "FipsCode");
            DropColumn("dbo.State", "CountryId");
            DropColumn("dbo.EducationLevel", "EducationId");
            DropColumn("dbo.EducationLevel", "Education_Id");
            DropColumn("dbo.Prefix", "Description");
            DropTable("dbo.City");
            DropTable("dbo.CityName");
            DropTable("dbo.County");
            DropTable("dbo.Zip");
            DropTable("dbo.ZipBranch");
            DropTable("dbo.ZipStreet");
            DropTable("dbo.ZipPlus4");
            DropTable("dbo.Country");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.County",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Description = c.String(),
                        FipsCode = c.String(maxLength: 5),
                        LegacyCode = c.String(maxLength: 4),
                        StateId = c.Guid(),
                        Population = c.Int(nullable: false),
                        PopulationPerSqaureMile = c.Decimal(nullable: false, storeType: "money"),
                        PopulationPercentageChange = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CityName",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Description = c.String(),
                        IsPreferred = c.Boolean(nullable: false),
                        CityId = c.Guid(),
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Prefix", "Description", c => c.String());
            AddColumn("dbo.EducationLevel", "Education_Id", c => c.Guid());
            AddColumn("dbo.EducationLevel", "EducationId", c => c.Guid());
            AddColumn("dbo.State", "CountryId", c => c.Guid());
            AddColumn("dbo.State", "FipsCode", c => c.String(maxLength: 2));
            AddColumn("dbo.State", "Description", c => c.String());
            AddColumn("dbo.State", "StateCode", c => c.String(maxLength: 4));
            DropForeignKey("dbo.EducationToLevel", "EducationLevelId", "dbo.EducationLevel");
            DropForeignKey("dbo.EducationToLevel", "EducationId", "dbo.Education");
            DropForeignKey("dbo.ConstituentPaymentPreference", "PaymentPreferenceId", "dbo.PaymentPreference");
            DropForeignKey("dbo.ConstituentPaymentPreference", "ConstituentId", "dbo.Constituent");
            DropIndex("dbo.EducationToLevel", new[] { "EducationLevelId" });
            DropIndex("dbo.EducationToLevel", new[] { "EducationId" });
            DropIndex("dbo.ConstituentPaymentPreference", new[] { "PaymentPreferenceId" });
            DropIndex("dbo.ConstituentPaymentPreference", new[] { "ConstituentId" });
            DropColumn("dbo.Prefix", "Descriptin");
            DropColumn("dbo.State", "Name");
            DropColumn("dbo.State", "Abbreviation");
            DropTable("dbo.EducationToLevel");
            DropTable("dbo.ConstituentPaymentPreference");
            RenameIndex(table: "dbo.PaymentPreference", name: "IX_Constituent_Id", newName: "IX_ConstituentId");
            RenameColumn(table: "dbo.PaymentPreference", name: "Constituent_Id", newName: "ConstituentId");
            CreateIndex("dbo.EducationLevel", "Education_Id");
            CreateIndex("dbo.EducationLevel", "EducationId");
            CreateIndex("dbo.ZipPlus4", "ZipStreetId");
            CreateIndex("dbo.ZipStreet", "ZipId");
            CreateIndex("dbo.ZipBranch", "ZipId");
            CreateIndex("dbo.Zip", "CityId");
            CreateIndex("dbo.County", "StateId");
            CreateIndex("dbo.CityName", "CityId");
            CreateIndex("dbo.City", "CountyId");
            CreateIndex("dbo.City", "StateId");
            CreateIndex("dbo.State", "CountryId");
            AddForeignKey("dbo.EducationLevel", "EducationId", "dbo.Education", "Id");
            AddForeignKey("dbo.EducationLevel", "Education_Id", "dbo.Education", "Id");
            AddForeignKey("dbo.State", "CountryId", "dbo.Country", "Id");
            AddForeignKey("dbo.ZipPlus4", "ZipStreetId", "dbo.ZipStreet", "Id");
            AddForeignKey("dbo.ZipStreet", "ZipId", "dbo.Zip", "Id");
            AddForeignKey("dbo.ZipBranch", "ZipId", "dbo.Zip", "Id");
            AddForeignKey("dbo.Zip", "CityId", "dbo.City", "Id");
            AddForeignKey("dbo.City", "StateId", "dbo.State", "Id");
            AddForeignKey("dbo.County", "StateId", "dbo.State", "Id");
            AddForeignKey("dbo.City", "CountyId", "dbo.County", "Id");
            AddForeignKey("dbo.CityName", "CityId", "dbo.City", "Id");
        }
    }
}

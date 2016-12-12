namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ConstituentPaymentPreference", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.ConstituentPaymentPreference", "PaymentPreferenceId", "dbo.PaymentPreference");
            DropForeignKey("dbo.EducationToLevel", "EducationId", "dbo.Education");
            DropForeignKey("dbo.EducationToLevel", "EducationLevelId", "dbo.EducationLevel");
            DropIndex("dbo.ConstituentPaymentPreference", new[] { "ConstituentId" });
            DropIndex("dbo.ConstituentPaymentPreference", new[] { "PaymentPreferenceId" });
            DropIndex("dbo.EducationToLevel", new[] { "EducationId" });
            DropIndex("dbo.EducationToLevel", new[] { "EducationLevelId" });
            RenameColumn(table: "dbo.PaymentPreference", name: "Constituent_Id", newName: "ConstituentId");
            RenameIndex(table: "dbo.PaymentPreference", name: "IX_Constituent_Id", newName: "IX_ConstituentId");
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
                        FipsCode = c.String(maxLength: 5),
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
            
            AddColumn("dbo.State", "StateCode", c => c.String(maxLength: 4));
            AddColumn("dbo.State", "Description", c => c.String());
            AddColumn("dbo.State", "FipsCode", c => c.String(maxLength: 2));
            AddColumn("dbo.State", "CountryId", c => c.Guid());
            AddColumn("dbo.EducationLevel", "EducationId", c => c.Guid());
            AddColumn("dbo.EducationLevel", "Education_Id", c => c.Guid());
            AddColumn("dbo.Prefix", "Description", c => c.String());
            CreateIndex("dbo.State", "CountryId");
            CreateIndex("dbo.EducationLevel", "EducationId");
            CreateIndex("dbo.EducationLevel", "Education_Id");
            AddForeignKey("dbo.State", "CountryId", "dbo.Country", "Id");
            AddForeignKey("dbo.EducationLevel", "Education_Id", "dbo.Education", "Id");
            AddForeignKey("dbo.EducationLevel", "EducationId", "dbo.Education", "Id");
            DropColumn("dbo.State", "Abbreviation");
            DropColumn("dbo.State", "Name");
            DropColumn("dbo.Prefix", "Descriptin");
            DropTable("dbo.ConstituentPaymentPreference");
            DropTable("dbo.EducationToLevel");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EducationToLevel",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        EducationId = c.Guid(),
                        EducationLevelId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConstituentPaymentPreference",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ConstituentId = c.Guid(),
                        PaymentPreferenceId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Prefix", "Descriptin", c => c.String());
            AddColumn("dbo.State", "Name", c => c.String());
            AddColumn("dbo.State", "Abbreviation", c => c.String());
            DropForeignKey("dbo.EducationLevel", "EducationId", "dbo.Education");
            DropForeignKey("dbo.EducationLevel", "Education_Id", "dbo.Education");
            DropForeignKey("dbo.State", "CountryId", "dbo.Country");
            DropForeignKey("dbo.ZipPlus4", "ZipStreetId", "dbo.ZipStreet");
            DropForeignKey("dbo.ZipStreet", "ZipId", "dbo.Zip");
            DropForeignKey("dbo.ZipBranch", "ZipId", "dbo.Zip");
            DropForeignKey("dbo.Zip", "CityId", "dbo.City");
            DropForeignKey("dbo.City", "StateId", "dbo.State");
            DropForeignKey("dbo.County", "StateId", "dbo.State");
            DropForeignKey("dbo.City", "CountyId", "dbo.County");
            DropForeignKey("dbo.CityName", "CityId", "dbo.City");
            DropIndex("dbo.EducationLevel", new[] { "Education_Id" });
            DropIndex("dbo.EducationLevel", new[] { "EducationId" });
            DropIndex("dbo.ZipPlus4", new[] { "ZipStreetId" });
            DropIndex("dbo.ZipStreet", new[] { "ZipId" });
            DropIndex("dbo.ZipBranch", new[] { "ZipId" });
            DropIndex("dbo.Zip", new[] { "CityId" });
            DropIndex("dbo.County", new[] { "StateId" });
            DropIndex("dbo.CityName", new[] { "CityId" });
            DropIndex("dbo.City", new[] { "CountyId" });
            DropIndex("dbo.City", new[] { "StateId" });
            DropIndex("dbo.State", new[] { "CountryId" });
            DropColumn("dbo.Prefix", "Description");
            DropColumn("dbo.EducationLevel", "Education_Id");
            DropColumn("dbo.EducationLevel", "EducationId");
            DropColumn("dbo.State", "CountryId");
            DropColumn("dbo.State", "FipsCode");
            DropColumn("dbo.State", "Description");
            DropColumn("dbo.State", "StateCode");
            DropTable("dbo.Country");
            DropTable("dbo.ZipPlus4");
            DropTable("dbo.ZipStreet");
            DropTable("dbo.ZipBranch");
            DropTable("dbo.Zip");
            DropTable("dbo.County");
            DropTable("dbo.CityName");
            DropTable("dbo.City");
            RenameIndex(table: "dbo.PaymentPreference", name: "IX_ConstituentId", newName: "IX_Constituent_Id");
            RenameColumn(table: "dbo.PaymentPreference", name: "ConstituentId", newName: "Constituent_Id");
            CreateIndex("dbo.EducationToLevel", "EducationLevelId");
            CreateIndex("dbo.EducationToLevel", "EducationId");
            CreateIndex("dbo.ConstituentPaymentPreference", "PaymentPreferenceId");
            CreateIndex("dbo.ConstituentPaymentPreference", "ConstituentId");
            AddForeignKey("dbo.EducationToLevel", "EducationLevelId", "dbo.EducationLevel", "Id");
            AddForeignKey("dbo.EducationToLevel", "EducationId", "dbo.Education", "Id");
            AddForeignKey("dbo.ConstituentPaymentPreference", "PaymentPreferenceId", "dbo.PaymentPreference", "Id");
            AddForeignKey("dbo.ConstituentPaymentPreference", "ConstituentId", "dbo.Constituent", "Id");
        }
    }
}

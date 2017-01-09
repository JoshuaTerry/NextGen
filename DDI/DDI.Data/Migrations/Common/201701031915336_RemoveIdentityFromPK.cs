namespace DDI.Data.Migrations.Common
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIdentityFromPK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CityName", "CityId", "dbo.City");
            DropForeignKey("dbo.Zip", "CityId", "dbo.City");
            DropForeignKey("dbo.City", "CountyId", "dbo.County");
            DropForeignKey("dbo.City", "StateId", "dbo.State");
            DropForeignKey("dbo.County", "StateId", "dbo.State");
            DropForeignKey("dbo.State", "CountryId", "dbo.Country");
            DropForeignKey("dbo.ZipBranch", "ZipId", "dbo.Zip");
            DropForeignKey("dbo.ZipStreet", "ZipId", "dbo.Zip");
            DropForeignKey("dbo.ZipPlus4", "ZipStreetId", "dbo.ZipStreet");
            DropPrimaryKey("dbo.Abbreviation");
            DropPrimaryKey("dbo.City");
            DropPrimaryKey("dbo.CityName");
            DropPrimaryKey("dbo.County");
            DropPrimaryKey("dbo.State");
            DropPrimaryKey("dbo.Country");
            DropPrimaryKey("dbo.Zip");
            DropPrimaryKey("dbo.ZipBranch");
            DropPrimaryKey("dbo.ZipStreet");
            DropPrimaryKey("dbo.ZipPlus4");
            AlterColumn("dbo.Abbreviation", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.City", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.CityName", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.County", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.State", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Country", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.Zip", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.ZipBranch", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.ZipStreet", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.ZipPlus4", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Abbreviation", "Id");
            AddPrimaryKey("dbo.City", "Id");
            AddPrimaryKey("dbo.CityName", "Id");
            AddPrimaryKey("dbo.County", "Id");
            AddPrimaryKey("dbo.State", "Id");
            AddPrimaryKey("dbo.Country", "Id");
            AddPrimaryKey("dbo.Zip", "Id");
            AddPrimaryKey("dbo.ZipBranch", "Id");
            AddPrimaryKey("dbo.ZipStreet", "Id");
            AddPrimaryKey("dbo.ZipPlus4", "Id");
            AddForeignKey("dbo.CityName", "CityId", "dbo.City", "Id");
            AddForeignKey("dbo.Zip", "CityId", "dbo.City", "Id");
            AddForeignKey("dbo.City", "CountyId", "dbo.County", "Id");
            AddForeignKey("dbo.City", "StateId", "dbo.State", "Id");
            AddForeignKey("dbo.County", "StateId", "dbo.State", "Id");
            AddForeignKey("dbo.State", "CountryId", "dbo.Country", "Id");
            AddForeignKey("dbo.ZipBranch", "ZipId", "dbo.Zip", "Id");
            AddForeignKey("dbo.ZipStreet", "ZipId", "dbo.Zip", "Id");
            AddForeignKey("dbo.ZipPlus4", "ZipStreetId", "dbo.ZipStreet", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZipPlus4", "ZipStreetId", "dbo.ZipStreet");
            DropForeignKey("dbo.ZipStreet", "ZipId", "dbo.Zip");
            DropForeignKey("dbo.ZipBranch", "ZipId", "dbo.Zip");
            DropForeignKey("dbo.State", "CountryId", "dbo.Country");
            DropForeignKey("dbo.County", "StateId", "dbo.State");
            DropForeignKey("dbo.City", "StateId", "dbo.State");
            DropForeignKey("dbo.City", "CountyId", "dbo.County");
            DropForeignKey("dbo.Zip", "CityId", "dbo.City");
            DropForeignKey("dbo.CityName", "CityId", "dbo.City");
            DropPrimaryKey("dbo.ZipPlus4");
            DropPrimaryKey("dbo.ZipStreet");
            DropPrimaryKey("dbo.ZipBranch");
            DropPrimaryKey("dbo.Zip");
            DropPrimaryKey("dbo.Country");
            DropPrimaryKey("dbo.State");
            DropPrimaryKey("dbo.County");
            DropPrimaryKey("dbo.CityName");
            DropPrimaryKey("dbo.City");
            DropPrimaryKey("dbo.Abbreviation");
            AlterColumn("dbo.ZipPlus4", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.ZipStreet", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.ZipBranch", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Zip", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Country", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.State", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.County", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.CityName", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.City", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Abbreviation", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.ZipPlus4", "Id");
            AddPrimaryKey("dbo.ZipStreet", "Id");
            AddPrimaryKey("dbo.ZipBranch", "Id");
            AddPrimaryKey("dbo.Zip", "Id");
            AddPrimaryKey("dbo.Country", "Id");
            AddPrimaryKey("dbo.State", "Id");
            AddPrimaryKey("dbo.County", "Id");
            AddPrimaryKey("dbo.CityName", "Id");
            AddPrimaryKey("dbo.City", "Id");
            AddPrimaryKey("dbo.Abbreviation", "Id");
            AddForeignKey("dbo.ZipPlus4", "ZipStreetId", "dbo.ZipStreet", "Id");
            AddForeignKey("dbo.ZipStreet", "ZipId", "dbo.Zip", "Id");
            AddForeignKey("dbo.ZipBranch", "ZipId", "dbo.Zip", "Id");
            AddForeignKey("dbo.State", "CountryId", "dbo.Country", "Id");
            AddForeignKey("dbo.County", "StateId", "dbo.State", "Id");
            AddForeignKey("dbo.City", "StateId", "dbo.State", "Id");
            AddForeignKey("dbo.City", "CountyId", "dbo.County", "Id");
            AddForeignKey("dbo.Zip", "CityId", "dbo.City", "Id");
            AddForeignKey("dbo.CityName", "CityId", "dbo.City", "Id");
        }
    }
}

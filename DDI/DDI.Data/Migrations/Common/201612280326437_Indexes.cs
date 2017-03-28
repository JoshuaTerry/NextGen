namespace DDI.Data.Migrations.Common
{
    using System.Data.Entity.Migrations;

    public partial class Indexes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CityName", "Description", c => c.String(maxLength: 128));
            CreateIndex("dbo.CityName", "Description");
            CreateIndex("dbo.County", "FIPSCode");
            CreateIndex("dbo.Zip", "ZipCode");
            CreateIndex("dbo.ZipBranch", "USPSKey");
            CreateIndex("dbo.ZipStreet", "CityKey");
            CreateIndex("dbo.ZipPlus4", "UpdateKey");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ZipPlus4", new[] { "UpdateKey" });
            DropIndex("dbo.ZipStreet", new[] { "CityKey" });
            DropIndex("dbo.ZipBranch", new[] { "USPSKey" });
            DropIndex("dbo.Zip", new[] { "ZipCode" });
            DropIndex("dbo.County", new[] { "FIPSCode" });
            DropIndex("dbo.CityName", new[] { "Description" });
            AlterColumn("dbo.CityName", "Description", c => c.String());
        }
    }
}

namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GL_v6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BusinessUnitFromTo", "OffsettingBusinessUnitId", "dbo.Fund");
            AddForeignKey("dbo.BusinessUnitFromTo", "OffsettingBusinessUnitId", "dbo.BusinessUnit", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BusinessUnitFromTo", "OffsettingBusinessUnitId", "dbo.BusinessUnit");
            AddForeignKey("dbo.BusinessUnitFromTo", "OffsettingBusinessUnitId", "dbo.Fund", "Id");
        }
    }
}

namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntityNumber : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EntityNumber", "BusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.EntityNumber", "FiscalYearId", "dbo.FiscalYear");
            DropIndex("dbo.EntityNumber", new[] { "BusinessUnitId" });
            DropIndex("dbo.EntityNumber", new[] { "FiscalYearId" });
            AddColumn("dbo.EntityNumber", "RangeId", c => c.Guid());
            DropColumn("dbo.EntityNumber", "BusinessUnitId");
            DropColumn("dbo.EntityNumber", "FiscalYearId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EntityNumber", "FiscalYearId", c => c.Guid());
            AddColumn("dbo.EntityNumber", "BusinessUnitId", c => c.Guid());
            DropColumn("dbo.EntityNumber", "RangeId");
            CreateIndex("dbo.EntityNumber", "FiscalYearId");
            CreateIndex("dbo.EntityNumber", "BusinessUnitId");
            AddForeignKey("dbo.EntityNumber", "FiscalYearId", "dbo.FiscalYear", "Id");
            AddForeignKey("dbo.EntityNumber", "BusinessUnitId", "dbo.BusinessUnit", "Id");
        }
    }
}

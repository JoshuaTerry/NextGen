namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GL_v3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Segment", "IX_Code");
            DropIndex("dbo.Segment", "IX_Name");
            CreateIndex("dbo.Segment", "FiscalYearId");
            CreateIndex("dbo.Segment", "SegmentLevelId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Segment", new[] { "SegmentLevelId" });
            DropIndex("dbo.Segment", new[] { "FiscalYearId" });
            CreateIndex("dbo.Segment", new[] { "FiscalYearId", "SegmentLevelId", "Name" }, unique: true, name: "IX_Name");
            CreateIndex("dbo.Segment", new[] { "FiscalYearId", "SegmentLevelId", "Code" }, unique: true, name: "IX_Code");
        }
    }
}

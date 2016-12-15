namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Region_v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        Code = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        ParentRegionId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Region", t => t.ParentRegionId)
                .Index(t => t.ParentRegionId);
            
            CreateTable(
                "dbo.RegionLevel",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Abbreviation = c.String(maxLength: 128),
                        Label = c.String(maxLength: 128),
                        Level = c.Int(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        IsChildLevel = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Address", "Region1Id", c => c.Guid());
            AddColumn("dbo.Address", "Region2Id", c => c.Guid());
            AddColumn("dbo.Address", "Region3Id", c => c.Guid());
            AddColumn("dbo.Address", "Region4Id", c => c.Guid());
            CreateIndex("dbo.Address", "Region1Id");
            CreateIndex("dbo.Address", "Region2Id");
            CreateIndex("dbo.Address", "Region3Id");
            CreateIndex("dbo.Address", "Region4Id");
            AddForeignKey("dbo.Address", "Region1Id", "dbo.Region", "Id");
            AddForeignKey("dbo.Address", "Region2Id", "dbo.Region", "Id");
            AddForeignKey("dbo.Address", "Region3Id", "dbo.Region", "Id");
            AddForeignKey("dbo.Address", "Region4Id", "dbo.Region", "Id");
        }
        
        public override void Down()
        {
                       
            DropForeignKey("dbo.Address", "Region4Id", "dbo.Region");
            DropForeignKey("dbo.Address", "Region3Id", "dbo.Region");
            DropForeignKey("dbo.Address", "Region2Id", "dbo.Region");
            DropForeignKey("dbo.Address", "Region1Id", "dbo.Region");
            DropForeignKey("dbo.Region", "ParentRegionId", "dbo.Region");
            DropIndex("dbo.Region", new[] { "ParentRegionId" });
            DropIndex("dbo.Address", new[] { "Region4Id" });
            DropIndex("dbo.Address", new[] { "Region3Id" });
            DropIndex("dbo.Address", new[] { "Region2Id" });
            DropIndex("dbo.Address", new[] { "Region1Id" });
            DropColumn("dbo.Address", "Region4Id");
            DropColumn("dbo.Address", "Region3Id");
            DropColumn("dbo.Address", "Region2Id");
            DropColumn("dbo.Address", "Region1Id");
            DropTable("dbo.RegionLevel");
            DropTable("dbo.Region");
        }
    }
}

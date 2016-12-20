namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EdLevel_DBA : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DoingBusinessAsConstituents", "DoingBusinessAs_Id", "dbo.DoingBusinessAs");
            DropForeignKey("dbo.DoingBusinessAsConstituents", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.EducationLevel", "Education_Id", "dbo.Education");
            DropIndex("dbo.EducationLevel", new[] { "Education_Id" });
            DropIndex("dbo.DoingBusinessAsConstituents", new[] { "DoingBusinessAs_Id" });
            DropIndex("dbo.DoingBusinessAsConstituents", new[] { "Constituent_Id" });
            RenameColumn(table: "dbo.Education", name: "Constituent_Id", newName: "ConstituentId");
            RenameIndex(table: "dbo.Education", name: "IX_Constituent_Id", newName: "IX_ConstituentId");
            AddColumn("dbo.Address", "AddressType_Id", c => c.Guid());
            AddColumn("dbo.DoingBusinessAs", "ConstituentId", c => c.Guid());
            CreateIndex("dbo.Address", "AddressType_Id");
            CreateIndex("dbo.DoingBusinessAs", "ConstituentId");
            AddForeignKey("dbo.Address", "AddressType_Id", "dbo.AddressType", "Id");
            AddForeignKey("dbo.DoingBusinessAs", "ConstituentId", "dbo.Constituent", "Id");
            DropColumn("dbo.EducationLevel", "Education_Id");
            DropTable("dbo.DoingBusinessAsConstituents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DoingBusinessAsConstituents",
                c => new
                    {
                        DoingBusinessAs_Id = c.Guid(nullable: false),
                        Constituent_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.DoingBusinessAs_Id, t.Constituent_Id });
            
            AddColumn("dbo.EducationLevel", "Education_Id", c => c.Guid());
            DropForeignKey("dbo.DoingBusinessAs", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.Address", "AddressType_Id", "dbo.AddressType");
            DropIndex("dbo.DoingBusinessAs", new[] { "ConstituentId" });
            DropIndex("dbo.Address", new[] { "AddressType_Id" });
            DropColumn("dbo.DoingBusinessAs", "ConstituentId");
            DropColumn("dbo.Address", "AddressType_Id");
            RenameIndex(table: "dbo.Education", name: "IX_ConstituentId", newName: "IX_Constituent_Id");
            RenameColumn(table: "dbo.Education", name: "ConstituentId", newName: "Constituent_Id");
            CreateIndex("dbo.DoingBusinessAsConstituents", "Constituent_Id");
            CreateIndex("dbo.DoingBusinessAsConstituents", "DoingBusinessAs_Id");
            CreateIndex("dbo.EducationLevel", "Education_Id");
            AddForeignKey("dbo.EducationLevel", "Education_Id", "dbo.Education", "Id");
            AddForeignKey("dbo.DoingBusinessAsConstituents", "Constituent_Id", "dbo.Constituent", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DoingBusinessAsConstituents", "DoingBusinessAs_Id", "dbo.DoingBusinessAs", "Id", cascadeDelete: true);
        }
    }
}

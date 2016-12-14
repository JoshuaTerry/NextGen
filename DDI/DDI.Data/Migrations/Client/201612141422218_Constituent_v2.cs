namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Constituent_v2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Constituent", name: "EducationLevel_Id", newName: "EducationLevelId");
            RenameIndex(table: "dbo.Constituent", name: "IX_EducationLevel_Id", newName: "IX_EducationLevelId");
            AddColumn("dbo.Constituent", "BirthDateType", c => c.Int(nullable: false));
            AddColumn("dbo.Constituent", "BirthMonth", c => c.Int());
            AddColumn("dbo.Constituent", "BirthDay", c => c.Int());
            AddColumn("dbo.Constituent", "CorrespondencePreference", c => c.Int(nullable: false));
            AddColumn("dbo.Constituent", "Name", c => c.String());
            AddColumn("dbo.Constituent", "NameFormat", c => c.String());
            AddColumn("dbo.Constituent", "SalutationType", c => c.Int(nullable: false));
            AlterColumn("dbo.Constituent", "DeceasedDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Constituent", "DivorceDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Constituent", "EmploymentEndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Constituent", "EmploymentStartDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Constituent", "FirstEmploymentDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Constituent", "MarriageDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Constituent", "ProspectDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Constituent", "TaxExemptVerifyDate", c => c.DateTime(storeType: "date"));
            DropColumn("dbo.Constituent", "BirthDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Constituent", "BirthDate", c => c.DateTime());
            AlterColumn("dbo.Constituent", "TaxExemptVerifyDate", c => c.DateTime());
            AlterColumn("dbo.Constituent", "ProspectDate", c => c.DateTime());
            AlterColumn("dbo.Constituent", "MarriageDate", c => c.DateTime());
            AlterColumn("dbo.Constituent", "FirstEmploymentDate", c => c.DateTime());
            AlterColumn("dbo.Constituent", "EmploymentStartDate", c => c.DateTime());
            AlterColumn("dbo.Constituent", "EmploymentEndDate", c => c.DateTime());
            AlterColumn("dbo.Constituent", "DivorceDate", c => c.DateTime());
            AlterColumn("dbo.Constituent", "DeceasedDate", c => c.DateTime());
            DropColumn("dbo.Constituent", "SalutationType");
            DropColumn("dbo.Constituent", "NameFormat");
            DropColumn("dbo.Constituent", "Name");
            DropColumn("dbo.Constituent", "CorrespondencePreference");
            DropColumn("dbo.Constituent", "BirthDay");
            DropColumn("dbo.Constituent", "BirthMonth");
            DropColumn("dbo.Constituent", "BirthDateType");
            RenameIndex(table: "dbo.Constituent", name: "IX_EducationLevelId", newName: "IX_EducationLevel_Id");
            RenameColumn(table: "dbo.Constituent", name: "EducationLevelId", newName: "EducationLevel_Id");
        }
    }
}

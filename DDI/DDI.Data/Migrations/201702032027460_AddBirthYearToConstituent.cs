namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBirthYearToConstituent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentPreference", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.CustomFieldData", "CustomFieldId", "dbo.CustomField");
            DropForeignKey("dbo.CustomFieldOption", "CustomFieldId", "dbo.CustomField");
            DropIndex("dbo.PaymentPreference", new[] { "ConstituentId" });
            DropPrimaryKey("dbo.CustomField");
            DropPrimaryKey("dbo.CustomFieldData");
            DropPrimaryKey("dbo.CustomFieldOption");
            AddColumn("dbo.Constituent", "BirthYear", c => c.Int());
            AlterColumn("dbo.CustomField", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.CustomFieldData", "Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.CustomFieldOption", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.CustomField", "Id");
            AddPrimaryKey("dbo.CustomFieldData", "Id");
            AddPrimaryKey("dbo.CustomFieldOption", "Id");
            AddForeignKey("dbo.CustomFieldData", "CustomFieldId", "dbo.CustomField", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CustomFieldOption", "CustomFieldId", "dbo.CustomField", "Id", cascadeDelete: true);
            DropColumn("dbo.Constituent", "AgeRangeFrom");
            DropColumn("dbo.Constituent", "AgeRangeTo");
            DropTable("dbo.PaymentPreference");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PaymentPreference",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConstituentId = c.Guid(),
                        Name = c.String(maxLength: 128),
                        ABANumber = c.Int(nullable: false),
                        AccountNumber = c.String(maxLength: 128),
                        AccountType = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Constituent", "AgeRangeTo", c => c.Int());
            AddColumn("dbo.Constituent", "AgeRangeFrom", c => c.Int());
            DropForeignKey("dbo.CustomFieldOption", "CustomFieldId", "dbo.CustomField");
            DropForeignKey("dbo.CustomFieldData", "CustomFieldId", "dbo.CustomField");
            DropPrimaryKey("dbo.CustomFieldOption");
            DropPrimaryKey("dbo.CustomFieldData");
            DropPrimaryKey("dbo.CustomField");
            AlterColumn("dbo.CustomFieldOption", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.CustomFieldData", "Id", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.CustomField", "Id", c => c.Guid(nullable: false, identity: true));
            DropColumn("dbo.Constituent", "BirthYear");
            AddPrimaryKey("dbo.CustomFieldOption", "Id");
            AddPrimaryKey("dbo.CustomFieldData", "Id");
            AddPrimaryKey("dbo.CustomField", "Id");
            CreateIndex("dbo.PaymentPreference", "ConstituentId");
            AddForeignKey("dbo.CustomFieldOption", "CustomFieldId", "dbo.CustomField", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CustomFieldData", "CustomFieldId", "dbo.CustomField", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PaymentPreference", "ConstituentId", "dbo.Constituent", "Id");
        }
    }
}

namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentMethod : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomField", "Answer_Id", "dbo.CustomFieldData");
            DropIndex("dbo.CustomField", new[] { "Answer_Id" });
            CreateTable(
                "dbo.PaymentMethod",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        StatusDate = c.DateTime(),
                        CardToken = c.String(maxLength: 128),
                        BankName = c.String(maxLength: 128),
                        BankAccount = c.String(maxLength: 64),
                        RoutingNumber = c.String(maxLength: 64),
                        AccountType = c.Int(),
                        EFTFormatId = c.Guid(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EFTFormat", t => t.EFTFormatId)
                .Index(t => t.EFTFormatId);
            
            CreateTable(
                "dbo.EFTFormat",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 4),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentMethodBaseConstituents",
                c => new
                    {
                        PaymentMethodBase_Id = c.Guid(nullable: false),
                        Constituent_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PaymentMethodBase_Id, t.Constituent_Id })
                .ForeignKey("dbo.PaymentMethod", t => t.PaymentMethodBase_Id, cascadeDelete: true)
                .ForeignKey("dbo.Constituent", t => t.Constituent_Id, cascadeDelete: true)
                .Index(t => t.PaymentMethodBase_Id)
                .Index(t => t.Constituent_Id);
            
            AddColumn("dbo.Constituent", "PreferredPaymentMethod", c => c.Int(nullable: false));
            CreateIndex("dbo.CustomFieldData", "CustomFieldId");
            AddForeignKey("dbo.CustomFieldData", "CustomFieldId", "dbo.CustomField", "Id", cascadeDelete: true);
            DropColumn("dbo.CustomField", "Answer_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomField", "Answer_Id", c => c.Guid());
            DropForeignKey("dbo.CustomFieldData", "CustomFieldId", "dbo.CustomField");
            DropForeignKey("dbo.PaymentMethod", "EFTFormatId", "dbo.EFTFormat");
            DropForeignKey("dbo.PaymentMethodBaseConstituents", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.PaymentMethodBaseConstituents", "PaymentMethodBase_Id", "dbo.PaymentMethod");
            DropIndex("dbo.PaymentMethodBaseConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.PaymentMethodBaseConstituents", new[] { "PaymentMethodBase_Id" });
            DropIndex("dbo.CustomFieldData", new[] { "CustomFieldId" });
            DropIndex("dbo.PaymentMethod", new[] { "EFTFormatId" });
            DropColumn("dbo.Constituent", "PreferredPaymentMethod");
            DropTable("dbo.PaymentMethodBaseConstituents");
            DropTable("dbo.EFTFormat");
            DropTable("dbo.PaymentMethod");
            CreateIndex("dbo.CustomField", "Answer_Id");
            AddForeignKey("dbo.CustomField", "Answer_Id", "dbo.CustomFieldData", "Id");
        }
    }
}

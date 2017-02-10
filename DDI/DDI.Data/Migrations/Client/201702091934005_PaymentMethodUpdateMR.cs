namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentMethodUpdateMR : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.PaymentMethod");
            RenameTable(name: "dbo.EFTPaymentMethod", newName: "PaymentMethod");
            RenameTable(name: "dbo.EFTPaymentMethodConstituents", newName: "PaymentMethodConstituents");
            DropForeignKey("dbo.CardPaymentMethodConstituents", "CardPaymentMethod_Id", "dbo.CardPaymentMethod");
            DropForeignKey("dbo.CardPaymentMethodConstituents", "Constituent_Id", "dbo.Constituent");
            DropForeignKey("dbo.Constituent", "PaymentMethodBase_Id", "dbo.PaymentMethod");
            DropIndex("dbo.Constituent", new[] { "PaymentMethodBase_Id" });
            DropIndex("dbo.CardPaymentMethodConstituents", new[] { "CardPaymentMethod_Id" });
            DropIndex("dbo.CardPaymentMethodConstituents", new[] { "Constituent_Id" });
            RenameColumn(table: "dbo.PaymentMethodConstituents", name: "EFTPaymentMethod_Id", newName: "PaymentMethod_Id");
            RenameIndex(table: "dbo.PaymentMethodConstituents", name: "IX_EFTPaymentMethod_Id", newName: "IX_PaymentMethod_Id");
            AddColumn("dbo.PaymentMethod", "CardToken", c => c.String(maxLength: 128));
            DropTable("dbo.CardPaymentMethod");
            DropTable("dbo.CardPaymentMethodConstituents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CardPaymentMethodConstituents",
                c => new
                    {
                        CardPaymentMethod_Id = c.Guid(nullable: false),
                        Constituent_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.CardPaymentMethod_Id, t.Constituent_Id });
            
            CreateTable(
                "dbo.PaymentMethod",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        StatusDate = c.DateTime(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CardPaymentMethod",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        StatusDate = c.DateTime(),
                        CardToken = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Constituent", "PaymentMethodBase_Id", c => c.Guid());
            DropColumn("dbo.PaymentMethod", "CardToken");
            RenameIndex(table: "dbo.PaymentMethodConstituents", name: "IX_PaymentMethod_Id", newName: "IX_EFTPaymentMethod_Id");
            RenameColumn(table: "dbo.PaymentMethodConstituents", name: "PaymentMethod_Id", newName: "EFTPaymentMethod_Id");
            CreateIndex("dbo.CardPaymentMethodConstituents", "Constituent_Id");
            CreateIndex("dbo.CardPaymentMethodConstituents", "CardPaymentMethod_Id");
            CreateIndex("dbo.Constituent", "PaymentMethodBase_Id");
            AddForeignKey("dbo.Constituent", "PaymentMethodBase_Id", "dbo.PaymentMethod", "Id");
            AddForeignKey("dbo.CardPaymentMethodConstituents", "Constituent_Id", "dbo.Constituent", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CardPaymentMethodConstituents", "CardPaymentMethod_Id", "dbo.CardPaymentMethod", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.PaymentMethodConstituents", newName: "EFTPaymentMethodConstituents");
            RenameTable(name: "dbo.PaymentMethod", newName: "EFTPaymentMethod");
        }
    }
}

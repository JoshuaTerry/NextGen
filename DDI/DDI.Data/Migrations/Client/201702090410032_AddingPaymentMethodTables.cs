namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingPaymentMethodTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EFTPaymentMethod",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(maxLength: 128),
                        Status = c.Int(nullable: false),
                        StatusDate = c.DateTime(),
                        BankName = c.String(maxLength: 128),
                        BankAccount = c.String(maxLength: 64),
                        RoutingNumber = c.String(maxLength: 64),
                        AccountType = c.Int(nullable: false),
                        EFTFormatId = c.Guid(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.EFTFormatId);
            
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
                .PrimaryKey(t => t.Id)
                .Index(t => t.CardToken);

            CreateTable(
                "dbo.EFTPaymentMethodConstituents",
                c => new
                {
                    EFTPaymentMethod_Id = c.Guid(nullable: false),
                    Constituent_Id = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.EFTPaymentMethod_Id, t.Constituent_Id })
                .ForeignKey("dbo.EFTPaymentMethod", t => t.EFTPaymentMethod_Id, cascadeDelete: true)
                .Index(t => t.EFTPaymentMethod_Id)
                .Index(t => t.Constituent_Id);

            CreateTable(
                "dbo.CardPaymentMethodConstituents",
                c => new
                {
                    CardPaymentMethod_Id = c.Guid(nullable: false),
                    Constituent_Id = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.CardPaymentMethod_Id, t.Constituent_Id })
                .ForeignKey("dbo.CardPaymentMethod", t => t.CardPaymentMethod_Id, cascadeDelete: true)
                .Index(t => t.CardPaymentMethod_Id)
                .Index(t => t.Constituent_Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EFTPaymentMethodConstituents", "EFTPaymentMethod_Id", "dbo.EFTPaymentMethod");
            DropForeignKey("dbo.CardPaymentMethodConstituents", "CardPaymentMethod_Id", "dbo.CardPaymentMethod");

            DropIndex("dbo.EFTPaymentMethodConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.EFTPaymentMethodConstituents", new[] { "EFTPaymentMethod_Id" });
            DropIndex("dbo.CardPaymentMethodConstituents", new[] { "Constituent_Id" });
            DropIndex("dbo.CardPaymentMethodConstituents", new[] { "CardPaymentMethod_Id" });
            DropIndex("dbo.EFTPaymentMethod", new[] { "EFTFormatId" });
            DropIndex("dbo.CardPaymentMethod", new[] { "CardToken" });

            DropTable("dbo.EFTPaymentMethodConstituents");
            DropTable("dbo.CardPaymentMethodConstituents");
            DropTable("dbo.EFTPaymentMethod");
            DropTable("dbo.CardPaymentMethod");
        }
    }
}

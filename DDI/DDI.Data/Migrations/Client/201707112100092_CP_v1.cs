namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CP_v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccount",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 128),
                        Code = c.String(maxLength: 4),
                        BusinessUnitId = c.Guid(),
                        DebitAccountId = c.Guid(),
                        CreditAccountId = c.Guid(),
                        BankAccountType = c.Int(nullable: false),
                        CompanyName = c.String(maxLength: 30),
                        BankName = c.String(maxLength: 128),
                        RoutingNumber = c.String(maxLength: 9),
                        AccountNumber = c.String(maxLength: 128),
                        OriginNumber = c.String(maxLength: 10),
                        OriginName = c.String(maxLength: 30),
                        DestinationNumber = c.String(maxLength: 10),
                        DestinationName = c.String(maxLength: 30),
                        CompanyIdNumber = c.String(maxLength: 10),
                        OriginatingFIDNumber = c.String(maxLength: 10),
                        FileIdModifier = c.String(maxLength: 1),
                        FractionalFormat = c.String(maxLength: 30),
                        GenerateBalancedACH = c.Boolean(nullable: false),
                        OffsetRoutingNumber = c.String(maxLength: 9),
                        OffsetAccountNumber = c.String(maxLength: 30),
                        OffsetDescription = c.String(maxLength: 30),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnitId)
                .ForeignKey("dbo.LedgerAccount", t => t.CreditAccountId)
                .ForeignKey("dbo.LedgerAccount", t => t.DebitAccountId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.BusinessUnitId)
                .Index(t => t.DebitAccountId)
                .Index(t => t.CreditAccountId);
            
            CreateTable(
                "dbo.DisbursementLine",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DisbursementId = c.Guid(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        LineType = c.Int(nullable: false),
                        Description = c.String(maxLength: 128),
                        DebitAmount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        CreditAmount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        InvoiceNumber = c.String(maxLength: 64),
                        InvoiceDate = c.DateTime(storeType: "date"),
                        ParentEntityId = c.Guid(),
                        EntityType = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disbursement", t => t.DisbursementId, cascadeDelete: true)
                .Index(t => t.DisbursementId)
                .Index(t => t.ParentEntityId)
                .Index(t => t.EntityType);
            
            CreateTable(
                "dbo.Disbursement",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DisbursementType = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        DisbursementDate = c.DateTime(storeType: "date"),
                        DisbursementNumber = c.Int(nullable: false),
                        DisbursementStatus = c.Int(nullable: false),
                        BankAccountId = c.Guid(),
                        LedgerAccountId = c.Guid(),
                        PayeeId = c.Guid(),
                        PayeeAddressId = c.Guid(),
                        FinalPayee = c.String(maxLength: 256),
                        TransactionDate = c.DateTime(storeType: "date"),
                        VoidDate = c.DateTime(storeType: "date"),
                        VoidComment = c.String(maxLength: 128),
                        FinalDate = c.DateTime(storeType: "date"),
                        FinalComment = c.String(maxLength: 512),
                        IsImmediate = c.Boolean(nullable: false),
                        IsManual = c.Boolean(nullable: false),
                        ModuleCode = c.String(maxLength: 4),
                        CheckForm = c.String(maxLength: 4),
                        ClearDate = c.DateTime(storeType: "date"),
                        BankName = c.String(maxLength: 128),
                        AccountNumber = c.String(maxLength: 64),
                        RoutingNumber = c.String(maxLength: 64),
                        EFTAccountType = c.Int(nullable: false),
                        EFTFormatId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccount", t => t.BankAccountId)
                .ForeignKey("dbo.EFTFormat", t => t.EFTFormatId)
                .ForeignKey("dbo.LedgerAccount", t => t.LedgerAccountId)
                .ForeignKey("dbo.Constituent", t => t.PayeeId)
                .ForeignKey("dbo.Address", t => t.PayeeAddressId)
                .Index(t => t.BankAccountId)
                .Index(t => t.LedgerAccountId)
                .Index(t => t.PayeeId)
                .Index(t => t.PayeeAddressId)
                .Index(t => t.EFTFormatId);
            
            CreateTable(
                "dbo.MiscReceiptLine",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        Comment = c.String(maxLength: 255),
                        LedgerAccountId = c.Guid(),
                        TransactionDate = c.DateTime(storeType: "date"),
                        DeletedOn = c.DateTime(storeType: "date"),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        MiscReceiptId = c.Guid(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LedgerAccount", t => t.LedgerAccountId)
                .ForeignKey("dbo.MiscReceipt", t => t.MiscReceiptId, cascadeDelete: true)
                .Index(t => t.LedgerAccountId)
                .Index(t => t.MiscReceiptId);
            
            CreateTable(
                "dbo.MiscReceipt",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MiscReceiptNumber = c.Int(nullable: false),
                        MiscReceiptType = c.Int(nullable: false),
                        BusinessUnitId = c.Guid(),
                        FiscalYearId = c.Guid(),
                        Comment = c.String(maxLength: 256),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        TransactionDate = c.DateTime(storeType: "date"),
                        IsReversed = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(storeType: "date"),
                        ConstituentId = c.Guid(),
                        DebitLedgerAccountId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnitId)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId)
                .ForeignKey("dbo.LedgerAccount", t => t.DebitLedgerAccountId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .Index(t => t.BusinessUnitId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.ConstituentId)
                .Index(t => t.DebitLedgerAccountId);
            
            CreateTable(
                "dbo.ReceiptBatch",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BusinessUnitId = c.Guid(),
                        BatchNumber = c.Int(nullable: false),
                        Name = c.String(maxLength: 256),
                        BankAccountId = c.Guid(),
                        BatchTypeId = c.Guid(),
                        Status = c.Int(nullable: false),
                        EntryMode = c.Int(nullable: false),
                        DistributionMode = c.Int(nullable: false),
                        EffectiveDate = c.DateTime(storeType: "date"),
                        TransactionDate = c.DateTime(storeType: "date"),
                        EnteredById = c.Guid(),
                        InUseById = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccount", t => t.BankAccountId)
                .ForeignKey("dbo.ReceiptBatchType", t => t.BatchTypeId)
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnitId)
                .ForeignKey("dbo.Users", t => t.EnteredById)
                .ForeignKey("dbo.Users", t => t.InUseById)
                .Index(t => t.BusinessUnitId)
                .Index(t => t.BankAccountId)
                .Index(t => t.BatchTypeId)
                .Index(t => t.EnteredById)
                .Index(t => t.InUseById);
            
            CreateTable(
                "dbo.ReceiptBatchType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        BankAccountId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankAccount", t => t.BankAccountId)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true)
                .Index(t => t.BankAccountId);
            
            CreateTable(
                "dbo.Receipt",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ReceiptNumber = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Reference = c.String(),
                        IsProcessed = c.Boolean(nullable: false),
                        IsReversed = c.Boolean(nullable: false),
                        AccountNumber = c.String(maxLength: 64),
                        RoutingNumber = c.String(maxLength: 64),
                        CheckNumber = c.String(maxLength: 30),
                        TransactionDate = c.DateTime(storeType: "date"),
                        ReceiptBatchId = c.Guid(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                        ReceiptType_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReceiptBatch", t => t.ReceiptBatchId, cascadeDelete: true)
                .ForeignKey("dbo.ReceiptType", t => t.ReceiptType_Id)
                .Index(t => t.ReceiptBatchId)
                .Index(t => t.ReceiptType_Id);
            
            CreateTable(
                "dbo.ReceiptType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                        Category = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Code, unique: true)
                .Index(t => t.Name, unique: true);
            
            DropColumn("dbo.EntityTransaction", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EntityTransaction", "Category", c => c.Int(nullable: false));
            DropForeignKey("dbo.Receipt", "ReceiptType_Id", "dbo.ReceiptType");
            DropForeignKey("dbo.Receipt", "ReceiptBatchId", "dbo.ReceiptBatch");
            DropForeignKey("dbo.ReceiptBatch", "InUseById", "dbo.Users");
            DropForeignKey("dbo.ReceiptBatch", "EnteredById", "dbo.Users");
            DropForeignKey("dbo.ReceiptBatch", "BusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.ReceiptBatch", "BatchTypeId", "dbo.ReceiptBatchType");
            DropForeignKey("dbo.ReceiptBatchType", "BankAccountId", "dbo.BankAccount");
            DropForeignKey("dbo.ReceiptBatch", "BankAccountId", "dbo.BankAccount");
            DropForeignKey("dbo.MiscReceiptLine", "MiscReceiptId", "dbo.MiscReceipt");
            DropForeignKey("dbo.MiscReceipt", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.MiscReceipt", "DebitLedgerAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.MiscReceipt", "ConstituentId", "dbo.Constituent");
            DropForeignKey("dbo.MiscReceipt", "BusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.MiscReceiptLine", "LedgerAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.Disbursement", "PayeeAddressId", "dbo.Address");
            DropForeignKey("dbo.Disbursement", "PayeeId", "dbo.Constituent");
            DropForeignKey("dbo.Disbursement", "LedgerAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.Disbursement", "EFTFormatId", "dbo.EFTFormat");
            DropForeignKey("dbo.DisbursementLine", "DisbursementId", "dbo.Disbursement");
            DropForeignKey("dbo.Disbursement", "BankAccountId", "dbo.BankAccount");
            DropForeignKey("dbo.BankAccount", "DebitAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.BankAccount", "CreditAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.BankAccount", "BusinessUnitId", "dbo.BusinessUnit");
            DropIndex("dbo.ReceiptType", new[] { "Name" });
            DropIndex("dbo.ReceiptType", new[] { "Code" });
            DropIndex("dbo.Receipt", new[] { "ReceiptType_Id" });
            DropIndex("dbo.Receipt", new[] { "ReceiptBatchId" });
            DropIndex("dbo.ReceiptBatchType", new[] { "BankAccountId" });
            DropIndex("dbo.ReceiptBatchType", new[] { "Name" });
            DropIndex("dbo.ReceiptBatchType", new[] { "Code" });
            DropIndex("dbo.ReceiptBatch", new[] { "InUseById" });
            DropIndex("dbo.ReceiptBatch", new[] { "EnteredById" });
            DropIndex("dbo.ReceiptBatch", new[] { "BatchTypeId" });
            DropIndex("dbo.ReceiptBatch", new[] { "BankAccountId" });
            DropIndex("dbo.ReceiptBatch", new[] { "BusinessUnitId" });
            DropIndex("dbo.MiscReceipt", new[] { "DebitLedgerAccountId" });
            DropIndex("dbo.MiscReceipt", new[] { "ConstituentId" });
            DropIndex("dbo.MiscReceipt", new[] { "FiscalYearId" });
            DropIndex("dbo.MiscReceipt", new[] { "BusinessUnitId" });
            DropIndex("dbo.MiscReceiptLine", new[] { "MiscReceiptId" });
            DropIndex("dbo.MiscReceiptLine", new[] { "LedgerAccountId" });
            DropIndex("dbo.Disbursement", new[] { "EFTFormatId" });
            DropIndex("dbo.Disbursement", new[] { "PayeeAddressId" });
            DropIndex("dbo.Disbursement", new[] { "PayeeId" });
            DropIndex("dbo.Disbursement", new[] { "LedgerAccountId" });
            DropIndex("dbo.Disbursement", new[] { "BankAccountId" });
            DropIndex("dbo.DisbursementLine", new[] { "EntityType" });
            DropIndex("dbo.DisbursementLine", new[] { "ParentEntityId" });
            DropIndex("dbo.DisbursementLine", new[] { "DisbursementId" });
            DropIndex("dbo.BankAccount", new[] { "CreditAccountId" });
            DropIndex("dbo.BankAccount", new[] { "DebitAccountId" });
            DropIndex("dbo.BankAccount", new[] { "BusinessUnitId" });
            DropIndex("dbo.BankAccount", new[] { "Code" });
            DropTable("dbo.ReceiptType");
            DropTable("dbo.Receipt");
            DropTable("dbo.ReceiptBatchType");
            DropTable("dbo.ReceiptBatch");
            DropTable("dbo.MiscReceipt");
            DropTable("dbo.MiscReceiptLine");
            DropTable("dbo.Disbursement");
            DropTable("dbo.DisbursementLine");
            DropTable("dbo.BankAccount");
        }
    }
}

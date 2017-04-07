namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Transaction_v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        TransactionNumber = c.Long(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        TransactionDate = c.DateTime(storeType: "date"),
                        PostDate = c.DateTime(),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        DebitAccountId = c.Guid(),
                        CreditAccountId = c.Guid(),
                        Status = c.Int(nullable: false),
                        IsAdjustment = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 255),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LedgerAccountYear", t => t.CreditAccountId)
                .ForeignKey("dbo.LedgerAccountYear", t => t.DebitAccountId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.DebitAccountId)
                .Index(t => t.CreditAccountId);
            
            CreateTable(
                "dbo.EntityTransaction",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Category = c.Int(nullable: false),
                        AmountType = c.Int(nullable: false),
                        TransactionId = c.Guid(),
                        ParentEntityId = c.Guid(),
                        EntityType = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Transaction", t => t.TransactionId)
                .Index(t => t.TransactionId)
                .Index(t => t.ParentEntityId)
                .Index(t => t.EntityType);
            
            CreateTable(
                "dbo.JournalLine",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        Comment = c.String(maxLength: 255),
                        LedgerAccountId = c.Guid(),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        Percent = c.Decimal(nullable: false, precision: 5, scale: 2),
                        DueToMode = c.Int(nullable: false),
                        SourceBusinessUnitId = c.Guid(),
                        SourceFundId = c.Guid(),
                        JournalId = c.Guid(nullable: false),
                        TransactionId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Journal", t => t.JournalId, cascadeDelete: true)
                .ForeignKey("dbo.LedgerAccount", t => t.LedgerAccountId)
                .ForeignKey("dbo.BusinessUnit", t => t.SourceBusinessUnitId)
                .ForeignKey("dbo.Fund", t => t.SourceFundId)
                .ForeignKey("dbo.Transaction", t => t.TransactionId)
                .Index(t => t.LedgerAccountId)
                .Index(t => t.SourceBusinessUnitId)
                .Index(t => t.SourceFundId)
                .Index(t => t.JournalId)
                .Index(t => t.TransactionId);
            
            CreateTable(
                "dbo.Journal",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        JournalNumber = c.Int(nullable: false),
                        JournalType = c.Int(nullable: false),
                        FiscalYearId = c.Guid(),
                        Comment = c.String(maxLength: 255),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        TransactionDate = c.DateTime(storeType: "date"),
                        ReverseOnDate = c.DateTime(storeType: "date"),
                        IsReversed = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RecurringType = c.Int(nullable: false),
                        RecurringDay = c.Int(nullable: false),
                        PreviousDate = c.DateTime(storeType: "date"),
                        IsExpired = c.Boolean(nullable: false),
                        ExpireAmount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        ExpireAmountTotal = c.Decimal(nullable: false, precision: 14, scale: 2),
                        ExpireDate = c.DateTime(storeType: "date"),
                        ExpireCount = c.Int(nullable: false),
                        ExpireCountTotal = c.Int(nullable: false),
                        ParentJournalId = c.Guid(),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Journal", t => t.ParentJournalId)
                .ForeignKey("dbo.FiscalYear", t => t.FiscalYearId)
                .Index(t => t.FiscalYearId)
                .Index(t => t.ParentJournalId);
            
            AddColumn("dbo.PostedTransaction", "PostedTransactionType", c => c.Int(nullable: false));
            AddColumn("dbo.PostedTransaction", "TransactionId", c => c.Guid());
            CreateIndex("dbo.PostedTransaction", "TransactionId");
            AddForeignKey("dbo.PostedTransaction", "TransactionId", "dbo.Transaction", "Id");
            DropColumn("dbo.PostedTransaction", "DocumentType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PostedTransaction", "DocumentType", c => c.String(maxLength: 4));
            DropForeignKey("dbo.JournalLine", "TransactionId", "dbo.Transaction");
            DropForeignKey("dbo.JournalLine", "SourceFundId", "dbo.Fund");
            DropForeignKey("dbo.JournalLine", "SourceBusinessUnitId", "dbo.BusinessUnit");
            DropForeignKey("dbo.JournalLine", "LedgerAccountId", "dbo.LedgerAccount");
            DropForeignKey("dbo.JournalLine", "JournalId", "dbo.Journal");
            DropForeignKey("dbo.Journal", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.Journal", "ParentJournalId", "dbo.Journal");
            DropForeignKey("dbo.PostedTransaction", "TransactionId", "dbo.Transaction");
            DropForeignKey("dbo.Transaction", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.EntityTransaction", "TransactionId", "dbo.Transaction");
            DropForeignKey("dbo.Transaction", "DebitAccountId", "dbo.LedgerAccountYear");
            DropForeignKey("dbo.Transaction", "CreditAccountId", "dbo.LedgerAccountYear");
            DropIndex("dbo.Journal", new[] { "ParentJournalId" });
            DropIndex("dbo.Journal", new[] { "FiscalYearId" });
            DropIndex("dbo.JournalLine", new[] { "TransactionId" });
            DropIndex("dbo.JournalLine", new[] { "JournalId" });
            DropIndex("dbo.JournalLine", new[] { "SourceFundId" });
            DropIndex("dbo.JournalLine", new[] { "SourceBusinessUnitId" });
            DropIndex("dbo.JournalLine", new[] { "LedgerAccountId" });
            DropIndex("dbo.EntityTransaction", new[] { "EntityType" });
            DropIndex("dbo.EntityTransaction", new[] { "ParentEntityId" });
            DropIndex("dbo.EntityTransaction", new[] { "TransactionId" });
            DropIndex("dbo.Transaction", new[] { "CreditAccountId" });
            DropIndex("dbo.Transaction", new[] { "DebitAccountId" });
            DropIndex("dbo.Transaction", new[] { "FiscalYearId" });
            DropIndex("dbo.PostedTransaction", new[] { "TransactionId" });
            DropColumn("dbo.PostedTransaction", "TransactionId");
            DropColumn("dbo.PostedTransaction", "PostedTransactionType");
            DropTable("dbo.Journal");
            DropTable("dbo.JournalLine");
            DropTable("dbo.EntityTransaction");
            DropTable("dbo.Transaction");
        }
    }
}

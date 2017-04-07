namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Transaction_v0 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubledgerTransaction", "CreditAccountId", "dbo.LedgerAccountYear");
            DropForeignKey("dbo.SubledgerTransaction", "DebitAccountId", "dbo.LedgerAccountYear");
            DropForeignKey("dbo.SubledgerTransaction", "FiscalYearId", "dbo.FiscalYear");
            DropForeignKey("dbo.PostedTransaction", "SubledgerTransactionId", "dbo.SubledgerTransaction");
            DropIndex("dbo.PostedTransaction", new[] { "SubledgerTransactionId" });
            DropIndex("dbo.SubledgerTransaction", new[] { "FiscalYearId" });
            DropIndex("dbo.SubledgerTransaction", new[] { "DebitAccountId" });
            DropIndex("dbo.SubledgerTransaction", new[] { "CreditAccountId" });
            DropColumn("dbo.PostedTransaction", "TransactionId");
            DropColumn("dbo.PostedTransaction", "SubledgerTransactionId");
            DropTable("dbo.SubledgerTransaction");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubledgerTransaction",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FiscalYearId = c.Guid(),
                        TransactionNumber = c.Long(nullable: false),
                        LineNumber = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                        TransactionDate = c.DateTime(storeType: "date"),
                        PostDate = c.DateTime(),
                        DocumentType = c.String(maxLength: 4),
                        Amount = c.Decimal(nullable: false, precision: 14, scale: 2),
                        DebitAccountId = c.Guid(),
                        CreditAccountId = c.Guid(),
                        Status = c.Int(nullable: false),
                        IsAdjustment = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PostedTransaction", "SubledgerTransactionId", c => c.Guid());
            AddColumn("dbo.PostedTransaction", "TransactionId", c => c.Int(nullable: false));
            CreateIndex("dbo.SubledgerTransaction", "CreditAccountId");
            CreateIndex("dbo.SubledgerTransaction", "DebitAccountId");
            CreateIndex("dbo.SubledgerTransaction", "FiscalYearId");
            CreateIndex("dbo.PostedTransaction", "SubledgerTransactionId");
            AddForeignKey("dbo.PostedTransaction", "SubledgerTransactionId", "dbo.SubledgerTransaction", "Id");
            AddForeignKey("dbo.SubledgerTransaction", "FiscalYearId", "dbo.FiscalYear", "Id");
            AddForeignKey("dbo.SubledgerTransaction", "DebitAccountId", "dbo.LedgerAccountYear", "Id");
            AddForeignKey("dbo.SubledgerTransaction", "CreditAccountId", "dbo.LedgerAccountYear", "Id");
        }
    }
}

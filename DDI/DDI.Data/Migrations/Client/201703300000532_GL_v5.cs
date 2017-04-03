namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    using Extensions;

    public partial class GL_v5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FiscalYear", "Ledger_Id", "dbo.Ledger");
            DropIndex("dbo.FiscalYear", new[] { "Ledger_Id" });
            DropColumn("dbo.FiscalYear", "Ledger_Id");
            this.CreateView("AccountBalanceByPeriod", 
                "WITH T AS (SELECT a.Id, pt.PeriodNumber, CASE WHEN pt.Amount < 0 THEN 'CR' ELSE 'DB' END AS DebitCredit, pt.Amount " +
                "FROM PostedTransaction AS pt INNER JOIN " +
                "LedgerAccountYear AS lay ON lay.Id = pt.LedgerAccountYearId INNER JOIN " +
                "Account AS a ON a.Id = lay.AccountId " +
                "WHERE (pt.TransactionType = 0)) " +
                "SELECT Id, PeriodNumber, DebitCredit, ABS(SUM(Amount)) AS TotalAmount " +
                "FROM T AS T_1 " +
                "GROUP BY Id, PeriodNumber, DebitCredit");            
        }
        
        public override void Down()
        {
            this.DropView("AccountBalanceByPeriod");
            AddColumn("dbo.FiscalYear", "Ledger_Id", c => c.Guid());
            CreateIndex("dbo.FiscalYear", "Ledger_Id");
            AddForeignKey("dbo.FiscalYear", "Ledger_Id", "dbo.Ledger");
        }
    }
}

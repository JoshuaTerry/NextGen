namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    using Extensions;

    public partial class GL_v7 : DbMigration
    {
        public override void Up()
        {
            this.CreateView("AccountGLSelector",
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
           
        }
    }
}

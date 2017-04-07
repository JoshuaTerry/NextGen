namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Text;
    using Extensions;

    public partial class GL_v7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ledger", "PostDaysInAdvance", c => c.Int(nullable: false));
            AddColumn("dbo.FiscalYear", "HasAdjustmentPeriod", c => c.Boolean(nullable: true));

            StringBuilder sqlStatement = new StringBuilder();
            sqlStatement.Append("select l.AccountGroup1Title + ': ' + ag1.Name as Level1, l.AccountGroup2Title + ': ' + ag2.Name as Level2, l.AccountGroup3Title + ': ' + ag3.Name as Level3, ");
            sqlStatement.Append("l.AccountGroup4Title + ': ' + ag4.Name as Level4, acct.AccountNumber,	acct.Id ");
            sqlStatement.Append("FROM Account acct ");
            sqlStatement.Append("LEFT Outer join AccountGroup ag1 on ag1.Id = acct.Group1Id ");
            sqlStatement.Append("left outer join AccountGroup ag2 on ag2.Id = acct.Group2Id ");
            sqlStatement.Append("left outer join AccountGroup ag3 on ag3.Id = acct.Group3Id ");
            sqlStatement.Append("left outer join AccountGroup ag4 on ag4.Id = acct.Group4Id ");
            sqlStatement.Append("inner join Ledger l on fy.LedgerId = l.Id ");
            sqlStatement.Append("group by ag1.Sequence, ag2.Sequence, Ag3.Sequence, Ag4.Sequence,");
            sqlStatement.Append("l.AccountGroup1Title + ': ' + ag1.Name,	l.AccountGroup2Title + ': ' + ag2.Name,	l.AccountGroup3Title + ': ' + ag3.Name,	l.AccountGroup4Title + ': ' + ag4.Name, ");
            sqlStatement.Append("acct.AccountNumber,	acct.Id");
            this.CreateView("GLAccountSelection",sqlStatement.ToString());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ledger", "PostDaysInAdvance");
            DropColumn("dbo.FiscalYear", "HasAdjustmentPeriod");
            this.DropView("GLAccountSelection");
        }
    }
}

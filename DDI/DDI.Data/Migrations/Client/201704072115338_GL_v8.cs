

namespace DDI.Data.Migrations.Client
{
    using System;
    using Extensions;
    using System.Data.Entity.Migrations;
    using System.Text;

    public partial class GL_v8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ledger", "PostDaysInAdvance", c => c.Int(nullable: false));
            AddColumn("dbo.FiscalYear", "HasAdjustmentPeriod", c => c.Boolean(nullable: true));

            StringBuilder sqlStatement = new StringBuilder();
            sqlStatement.Append("select l.AccountGroup1Title + ': ' + ag1.Name as Level1, l.AccountGroup2Title + ': ' + ag2.Name as Level2, ");
            sqlStatement.Append("l.AccountGroup3Title + ': ' + ag3.Name as Level3, l.AccountGroup4Title + ': ' + ag4.Name as Level4, ");
            sqlStatement.Append("acct.AccountNumber, acct.Name as Description,	acct.Id, l.Id as LedgerId, ag1.Sequence as LevelSequence1, ag2.Sequence as LevelSequence2, ");
            sqlStatement.Append("Ag3.Sequence as LevelSequence3, Ag4.Sequence as LevelSequence4, ");
            sqlStatement.Append("fy.Id as FiscalYearId, acct.SortKey as SortKey ");
            sqlStatement.Append("FROM Account acct ");
            sqlStatement.Append("LEFT Outer join AccountGroup ag1 on ag1.Id = acct.Group1Id ");
            sqlStatement.Append("left outer join AccountGroup ag2 on ag2.Id = acct.Group2Id ");
            sqlStatement.Append("left outer join AccountGroup ag3 on ag3.Id = acct.Group3Id ");
            sqlStatement.Append("left outer join AccountGroup ag4 on ag4.Id = acct.Group4Id ");
            sqlStatement.Append("left outer join FiscalYear fy on fy.Id = acct.FiscalYearId ");
            sqlStatement.Append("inner join Ledger l on fy.LedgerId = l.Id ");
            sqlStatement.Append("group by ag1.Sequence, ag2.Sequence, Ag3.Sequence, Ag4.Sequence, ");
            sqlStatement.Append("l.AccountGroup1Title + ': ' + ag1.Name, ");
            sqlStatement.Append("l.AccountGroup2Title + ': ' + ag2.Name, ");
            sqlStatement.Append("l.AccountGroup3Title + ': ' + ag3.Name, ");
            sqlStatement.Append("l.AccountGroup4Title + ': ' + ag4.Name, ");
            sqlStatement.Append("acct.SortKey, ");
            sqlStatement.Append("acct.AccountNumber, ");
            sqlStatement.Append("acct.Id, ");
            sqlStatement.Append("l.Id, ");
            sqlStatement.Append("fy.Id, ");
            sqlStatement.Append("acct.Name ");
            this.CreateView("GLAccountSelection", sqlStatement.ToString());
        }

        public override void Down()
        {
            DropColumn("dbo.Ledger", "PostDaysInAdvance");
            DropColumn("dbo.FiscalYear", "HasAdjustmentPeriod");
            this.DropView("GLAccountSelection");
        }
    }
}

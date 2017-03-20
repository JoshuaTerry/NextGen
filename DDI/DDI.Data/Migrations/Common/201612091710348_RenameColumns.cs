namespace DDI.Data.Migrations.Common
{
    using System.Data.Entity.Migrations;

    public partial class RenameColumns : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.ZipPlus4", "SecondaryAbbrev", "SecondaryAbbreviation");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.ZipPlus4", "SecondaryAbbreviation", "SecondaryAbbrev");
        }
    }
}

namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Constituent_v6 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Constituent", "ConstituentNum", "ConstituentNumber");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Constituent", "ConstituentNumber", "ConstituentNum");
        }
    }
}

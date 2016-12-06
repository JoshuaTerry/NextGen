namespace EFTest1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZipPlus4IsRange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ZipPlus4", "IsRange", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ZipPlus4", "IsRange");
        }
    }
}

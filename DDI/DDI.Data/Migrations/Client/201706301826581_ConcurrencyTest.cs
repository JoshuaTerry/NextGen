namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConcurrencyTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Constituent", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Constituent", "RowVersion");
        }
    }
}

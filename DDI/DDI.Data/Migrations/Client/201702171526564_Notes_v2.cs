namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notes_v2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Constituent", "BirthDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Notes", "AlertStartDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Notes", "AlertEndDate", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Notes", "ContactDate", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notes", "ContactDate", c => c.DateTime());
            AlterColumn("dbo.Notes", "AlertEndDate", c => c.DateTime());
            AlterColumn("dbo.Notes", "AlertStartDate", c => c.DateTime());
            AlterColumn("dbo.Constituent", "BirthDate", c => c.DateTime());
        }
    }
}

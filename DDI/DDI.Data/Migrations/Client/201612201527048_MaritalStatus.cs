namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MaritalStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MaritialStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SectionPreference",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        SectionName = c.String(maxLength: 128),
                        Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SectionPreference");
            DropTable("dbo.MaritialStatus");
        }
    }
}

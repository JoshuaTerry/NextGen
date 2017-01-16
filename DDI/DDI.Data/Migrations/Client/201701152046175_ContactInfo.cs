namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactInfo", "ParentContactId", c => c.Guid());
            CreateIndex("dbo.ContactInfo", "ParentContactId");
            AddForeignKey("dbo.ContactInfo", "ParentContactId", "dbo.ContactInfo", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactInfo", "ParentContactId", "dbo.ContactInfo");
            DropIndex("dbo.ContactInfo", new[] { "ParentContactId" });
            DropColumn("dbo.ContactInfo", "ParentContactId");
        }
    }
}

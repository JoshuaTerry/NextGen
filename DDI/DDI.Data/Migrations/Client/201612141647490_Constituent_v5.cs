namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Constituent_v5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Address", "AddressLine1", c => c.String(maxLength: 255));
            AddColumn("dbo.Address", "AddressLine2", c => c.String(maxLength: 255));
            DropColumn("dbo.Address", "StreetAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Address", "StreetAddress", c => c.String(maxLength: 255));
            DropColumn("dbo.Address", "AddressLine2");
            DropColumn("dbo.Address", "AddressLine1");
        }
    }
}

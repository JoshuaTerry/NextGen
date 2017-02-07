namespace DDI.Data.Migrations.Common
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertiesToConstituent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Abbreviation", "CreatedBy", c => c.Guid());
            AddColumn("dbo.Abbreviation", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Abbreviation", "LastModifiedBy", c => c.Guid());
            AddColumn("dbo.Abbreviation", "LastModifiedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Abbreviation", "LastModifiedOn");
            DropColumn("dbo.Abbreviation", "LastModifiedBy");
            DropColumn("dbo.Abbreviation", "CreatedOn");
            DropColumn("dbo.Abbreviation", "CreatedBy");
        }
    }
}

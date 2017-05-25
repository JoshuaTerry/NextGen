namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingGroupToAuditable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "CreatedBy", c => c.String());
            AddColumn("dbo.Groups", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.Groups", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Groups", "LastModifiedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "LastModifiedOn");
            DropColumn("dbo.Groups", "LastModifiedBy");
            DropColumn("dbo.Groups", "CreatedOn");
            DropColumn("dbo.Groups", "CreatedBy");
        }
    }
}

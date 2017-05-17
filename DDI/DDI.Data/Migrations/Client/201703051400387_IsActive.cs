namespace DDI.Data.Migrations.Client
{
    using System.Data.Entity.Migrations;

    public partial class IsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactCategory", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Gender", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Prefix", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Region", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Region", "IsActive");
            DropColumn("dbo.Prefix", "IsActive");
            DropColumn("dbo.Gender", "IsActive");
            DropColumn("dbo.ContactCategory", "IsActive");
        }
    }
}

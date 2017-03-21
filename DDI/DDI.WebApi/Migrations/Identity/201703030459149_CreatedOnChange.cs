namespace DDI.WebApi.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreatedOnChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Roles", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.UserRoles", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.Users", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.UserClaims", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
            AlterColumn("dbo.UserLogins", "CreatedOn", c => c.DateTime(defaultValueSql: "GETUTCDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserLogins", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserClaims", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Users", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserRoles", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Roles", "CreatedOn", c => c.DateTime());
        }
    }
}

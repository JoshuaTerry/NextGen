namespace DDI.WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserChangesForSomeUnknownReason : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Users", "FullName", c => c.String(maxLength: 256));
            AlterColumn("dbo.Roles", "CreatedBy", c => c.String());
            AlterColumn("dbo.Roles", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Roles", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserRoles", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserRoles", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserRoles", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.Users", "CreatedBy", c => c.String());
            AlterColumn("dbo.Users", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Users", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserClaims", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserClaims", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserClaims", "LastModifiedBy", c => c.String());
            AlterColumn("dbo.UserLogins", "CreatedBy", c => c.String());
            AlterColumn("dbo.UserLogins", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserLogins", "LastModifiedBy", c => c.String());
            //DropColumn("dbo.Users", "FirstName");
            //DropColumn("dbo.Users", "MiddleName");
            //DropColumn("dbo.Users", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "LastName", c => c.String(maxLength: 256));
            AddColumn("dbo.Users", "MiddleName", c => c.String(maxLength: 256));
            AddColumn("dbo.Users", "FirstName", c => c.String(maxLength: 256));
            AlterColumn("dbo.UserLogins", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.UserLogins", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserLogins", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.UserClaims", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.UserClaims", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserClaims", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Users", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Users", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Users", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.UserRoles", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.UserRoles", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.UserRoles", "CreatedBy", c => c.Guid());
            AlterColumn("dbo.Roles", "LastModifiedBy", c => c.Guid());
            AlterColumn("dbo.Roles", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Roles", "CreatedBy", c => c.Guid());
            DropColumn("dbo.Users", "FullName");
        }
    }
}

namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingGroups : DbMigration
    {
        public override void Up()
        {
            //RenameColumn(table: "dbo.Roles", name: "Id", newName: "RoleId");
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleGroups",
                c => new
                    {
                        Role_Id = c.Guid(nullable: false),
                        Group_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Group_Id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        User_Id = c.Guid(nullable: false),
                        Group_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Group_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Group_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RoleGroups", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.RoleGroups", "Role_Id", "dbo.Roles");
            DropIndex("dbo.UserGroups", new[] { "Group_Id" });
            DropIndex("dbo.UserGroups", new[] { "User_Id" });
            DropIndex("dbo.RoleGroups", new[] { "Group_Id" });
            DropIndex("dbo.RoleGroups", new[] { "Role_Id" });
            DropTable("dbo.UserGroups");
            DropTable("dbo.RoleGroups");
            DropTable("dbo.Groups");
            RenameColumn(table: "dbo.Roles", name: "RoleId", newName: "Id");
        }
    }
}

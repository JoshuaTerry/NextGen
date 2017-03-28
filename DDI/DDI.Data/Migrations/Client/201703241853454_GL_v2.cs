namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GL_v2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BusinessUnitUsers",
                c => new
                    {
                        BusinessUnit_Id = c.Guid(nullable: false),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.BusinessUnit_Id, t.User_Id })
                .ForeignKey("dbo.BusinessUnit", t => t.BusinessUnit_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.BusinessUnit_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BusinessUnitUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.BusinessUnitUsers", "BusinessUnit_Id", "dbo.BusinessUnit");
            DropIndex("dbo.BusinessUnitUsers", new[] { "User_Id" });
            DropIndex("dbo.BusinessUnitUsers", new[] { "BusinessUnit_Id" });
            DropTable("dbo.BusinessUnitUsers");
        }
    }
}

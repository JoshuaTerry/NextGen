namespace DDI.Data.Migrations.Client
{
    using System.Data.Entity.Migrations;

    public partial class ConstituentPictureTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConstituentPicture",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConstituentId = c.Guid(nullable: false),
                        FileId = c.Guid(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Constituent", t => t.ConstituentId, cascadeDelete: true)
                .ForeignKey("dbo.FileStorage", t => t.FileId, cascadeDelete: true)
                .Index(t => t.ConstituentId)
                .Index(t => t.FileId);   
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConstituentPicture", "FileId", "dbo.FileStorage");
            DropForeignKey("dbo.ConstituentPicture", "ConstituentId", "dbo.Constituent");
            DropIndex("dbo.ConstituentPicture", new[] { "FileId" });
            DropIndex("dbo.ConstituentPicture", new[] { "ConstituentId" });
            DropTable("dbo.ConstituentPicture");
        }
    }
}

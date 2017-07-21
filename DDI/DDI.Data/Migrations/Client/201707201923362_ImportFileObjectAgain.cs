namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportFileObjectAgain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImportFile",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ContainsHeaders = c.Boolean(nullable: false),
                        FileId = c.Guid(nullable: false),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileStorage", t => t.FileId, cascadeDelete: true)
                .Index(t => t.FileId);
            
            //AddColumn("dbo.GLAccountSelection", "LedgerAccountId", c => c.Guid(nullable: false));
            //DropColumn("dbo.GLAccountSelection", "RowVersion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GLAccountSelection", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            DropForeignKey("dbo.ImportFile", "FileId", "dbo.FileStorage");
            DropIndex("dbo.ImportFile", new[] { "FileId" });
            DropColumn("dbo.GLAccountSelection", "LedgerAccountId");
            DropTable("dbo.ImportFile");
        }
    }
}

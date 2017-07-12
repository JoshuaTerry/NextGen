namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileStorageChange : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PostedTransaction", "IX_TransactionNumber");
            DropIndex("dbo.Transaction", "IX_TransactionNumber");
            AddColumn("dbo.FileStorage", "FileType", c => c.String(maxLength: 128));
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Groups", "Name", c => c.String(maxLength: 128));
            DropColumn("dbo.FileStorage", "FileType");
            CreateIndex("dbo.Transaction", new[] { "TransactionNumber", "LineNumber" }, unique: true, name: "IX_TransactionNumber");
            CreateIndex("dbo.PostedTransaction", new[] { "TransactionNumber", "LineNumber" }, unique: true, name: "IX_TransactionNumber");
        }
    }
}

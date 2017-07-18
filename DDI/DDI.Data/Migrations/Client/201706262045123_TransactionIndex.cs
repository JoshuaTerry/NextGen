namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PostedTransaction", new[] { "TransactionNumber", "LineNumber" }, unique: true, name: "IX_TransactionNumber");
            CreateIndex("dbo.Transaction", new[] { "TransactionNumber", "LineNumber" }, unique: true, name: "IX_TransactionNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Transaction", "IX_TransactionNumber");
            DropIndex("dbo.PostedTransaction", "IX_TransactionNumber");
        }
    }
}

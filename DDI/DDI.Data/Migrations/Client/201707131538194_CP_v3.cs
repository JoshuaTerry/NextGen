namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CP_v3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Receipt", name: "ReceiptType_Id", newName: "ReceiptTypeId");
            RenameIndex(table: "dbo.Receipt", name: "IX_ReceiptType_Id", newName: "IX_ReceiptTypeId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Receipt", name: "IX_ReceiptTypeId", newName: "IX_ReceiptType_Id");
            RenameColumn(table: "dbo.Receipt", name: "ReceiptTypeId", newName: "ReceiptType_Id");
        }
    }
}

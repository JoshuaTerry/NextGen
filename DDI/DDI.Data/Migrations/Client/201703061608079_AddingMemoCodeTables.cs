namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMemoCodeTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemoCategory",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 128),
                        Code = c.String(maxLength: 4),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(defaultValueSql: "GETUTCDATE()"),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MemoCode",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 128),
                        Code = c.String(maxLength: 4),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(defaultValueSql: "GETUTCDATE()"),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MemoCode");
            DropTable("dbo.MemoCategory");
        }
    }
}

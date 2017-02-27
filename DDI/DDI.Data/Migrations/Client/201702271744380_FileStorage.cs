namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileStorage : DbMigration
    {
        public override void Up()
        {
            /*CreateTable(
                "dbo.FileStorage",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Extension = c.String(maxLength: 256),
                        Size = c.Long(nullable: false),
                        Data = c.Binary(),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            */
        }
        
        public override void Down()
        {
            DropTable("dbo.FileStorage");
        }
    }
}

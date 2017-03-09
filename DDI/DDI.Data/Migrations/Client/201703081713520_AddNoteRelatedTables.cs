namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNoteRelatedTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NoteCode",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Name = c.String(maxLength: 128),
                        Code = c.String(maxLength: 4),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.NoteCategories", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NoteCategories", "IsActive");
            DropTable("dbo.NoteCode");
        }
    }
}

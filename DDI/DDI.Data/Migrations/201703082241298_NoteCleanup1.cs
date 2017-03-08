namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoteCleanup1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Relationsihp", newName: "Relationship");
            RenameTable(name: "dbo.NoteCategories", newName: "NoteCategory");
            RenameTable(name: "dbo.Notes", newName: "Note");
            RenameTable(name: "dbo.NoteContactMethods", newName: "NoteContactMethod");
            RenameTable(name: "dbo.NoteTopics", newName: "NoteTopic");
            DropColumn("dbo.NoteCategory", "IsActive");
            DropTable("dbo.MemoCategory");
            DropTable("dbo.MemoCode");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MemoCode",
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
            
            CreateTable(
                "dbo.MemoCategory",
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
            
            AddColumn("dbo.NoteCategory", "IsActive", c => c.Boolean(nullable: false));
            RenameTable(name: "dbo.NoteTopic", newName: "NoteTopics");
            RenameTable(name: "dbo.NoteContactMethod", newName: "NoteContactMethods");
            RenameTable(name: "dbo.Note", newName: "Notes");
            RenameTable(name: "dbo.NoteCategory", newName: "NoteCategories");
            RenameTable(name: "dbo.Relationship", newName: "Relationsihp");
        }
    }
}

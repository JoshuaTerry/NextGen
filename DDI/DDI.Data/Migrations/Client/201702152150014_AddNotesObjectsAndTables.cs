namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotesObjectsAndTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NoteCategories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Label = c.String(maxLength: 64),
                        Name = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(maxLength: 256),
                        Text = c.String(),
                        AlertStartDate = c.DateTime(),
                        AlertEndDate = c.DateTime(),
                        ContactDate = c.DateTime(),
                        CategoryId = c.Guid(),
                        NoteCode = c.String(maxLength: 32),
                        PrimaryContactId = c.Guid(),
                        ContactMethodId = c.Guid(),
                        ParentEntityId = c.Guid(),
                        EntityType = c.String(maxLength: 128),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NoteCategories", t => t.CategoryId)
                .ForeignKey("dbo.NoteContactMethods", t => t.ContactMethodId)
                .ForeignKey("dbo.Constituent", t => t.PrimaryContactId)
                .Index(t => t.CategoryId)
                .Index(t => t.PrimaryContactId)
                .Index(t => t.ContactMethodId)
                .Index(t => t.ParentEntityId)
                .Index(t => t.EntityType);
            
            CreateTable(
                "dbo.NoteContactMethods",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 64),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NoteTopics",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(maxLength: 64),
                        Name = c.String(maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Guid(),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.Guid(),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NoteTopicNotes",
                c => new
                    {
                        NoteTopic_Id = c.Guid(nullable: false),
                        Note_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.NoteTopic_Id, t.Note_Id })
                .ForeignKey("dbo.NoteTopics", t => t.NoteTopic_Id, cascadeDelete: true)
                .ForeignKey("dbo.Notes", t => t.Note_Id, cascadeDelete: true)
                .Index(t => t.NoteTopic_Id)
                .Index(t => t.Note_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "PrimaryContactId", "dbo.Constituent");
            DropForeignKey("dbo.NoteTopicNotes", "Note_Id", "dbo.Notes");
            DropForeignKey("dbo.NoteTopicNotes", "NoteTopic_Id", "dbo.NoteTopics");
            DropForeignKey("dbo.Notes", "ContactMethodId", "dbo.NoteContactMethods");
            DropForeignKey("dbo.Notes", "CategoryId", "dbo.NoteCategories");
            DropIndex("dbo.NoteTopicNotes", new[] { "Note_Id" });
            DropIndex("dbo.NoteTopicNotes", new[] { "NoteTopic_Id" });
            DropIndex("dbo.Notes", new[] { "EntityType" });
            DropIndex("dbo.Notes", new[] { "ParentEntityId" });
            DropIndex("dbo.Notes", new[] { "ContactMethodId" });
            DropIndex("dbo.Notes", new[] { "PrimaryContactId" });
            DropIndex("dbo.Notes", new[] { "CategoryId" });
            DropTable("dbo.NoteTopicNotes");
            DropTable("dbo.NoteTopics");
            DropTable("dbo.NoteContactMethods");
            DropTable("dbo.Notes");
            DropTable("dbo.NoteCategories");
        }
    }
}

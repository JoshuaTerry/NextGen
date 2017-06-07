namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attachments_v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachment", "NoteId", c => c.Guid());
            CreateIndex("dbo.Attachment", "NoteId");
            AddForeignKey("dbo.Attachment", "NoteId", "dbo.Note", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachment", "NoteId", "dbo.Note");
            DropIndex("dbo.Attachment", new[] { "NoteId" });
            DropColumn("dbo.Attachment", "NoteId");
        }
    }
}

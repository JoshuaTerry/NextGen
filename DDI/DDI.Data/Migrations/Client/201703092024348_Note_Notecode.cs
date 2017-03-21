namespace DDI.Data.Migrations.Client
{
    using System.Data.Entity.Migrations;

    public partial class Note_Notecode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Note", "NoteCodeId", c => c.Guid());
            CreateIndex("dbo.Note", "NoteCodeId");
            AddForeignKey("dbo.Note", "NoteCodeId", "dbo.NoteCode", "Id");
            DropColumn("dbo.Note", "NoteCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Note", "NoteCode", c => c.String(maxLength: 32));
            DropForeignKey("dbo.Note", "NoteCodeId", "dbo.NoteCode");
            DropIndex("dbo.Note", new[] { "NoteCodeId" });
            DropColumn("dbo.Note", "NoteCodeId");
        }
    }
}

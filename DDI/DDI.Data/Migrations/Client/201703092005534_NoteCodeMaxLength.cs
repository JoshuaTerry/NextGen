namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoteCodeMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NoteCode", "Code", c => c.String(maxLength: 16));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NoteCode", "Code", c => c.String(maxLength: 4));
        }
    }
}

namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntityMappingNameUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SavedEntityMappings", "Name", c => c.String(maxLength: 128));
            CreateIndex("dbo.SavedEntityMappings", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.SavedEntityMappings", new[] { "Name" });
            AlterColumn("dbo.SavedEntityMappings", "Name", c => c.String());
        }
    }
}

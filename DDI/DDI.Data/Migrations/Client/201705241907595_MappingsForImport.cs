namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MappingsForImport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EntityMappings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MappingType = c.Int(nullable: false),
                        PropertyName = c.String(),
                        PropertyValue = c.String(),
                        PropertyType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SavedEntityMappings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        MappingType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SavedEntityMappingFields",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SavedEntityMappingId = c.Guid(nullable: false),
                        EntityMappingId = c.Guid(nullable: false),
                        ColumnName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntityMappings", t => t.EntityMappingId, cascadeDelete: true)
                .ForeignKey("dbo.SavedEntityMappings", t => t.SavedEntityMappingId, cascadeDelete: true)
                .Index(t => t.SavedEntityMappingId)
                .Index(t => t.EntityMappingId);
            
            
        }
        
        public override void Down()
        {
            
            DropForeignKey("dbo.SavedEntityMappingFields", "SavedEntityMappingId", "dbo.SavedEntityMappings");
            DropForeignKey("dbo.SavedEntityMappingFields", "EntityMappingId", "dbo.EntityMappings");
            DropIndex("dbo.SavedEntityMappingFields", new[] { "EntityMappingId" });
            DropIndex("dbo.SavedEntityMappingFields", new[] { "SavedEntityMappingId" });
            DropTable("dbo.SavedEntityMappingFields");
            DropTable("dbo.SavedEntityMappings");
            DropTable("dbo.EntityMappings");
        }
    }
}

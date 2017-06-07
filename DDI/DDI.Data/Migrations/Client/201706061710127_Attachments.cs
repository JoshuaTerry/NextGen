namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attachments : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EntityMappings", newName: "EntityMapping");
            RenameTable(name: "dbo.SavedEntityMappings", newName: "SavedEntityMapping");
            RenameTable(name: "dbo.SavedEntityMappingFields", newName: "SavedEntityMappingField");
            RenameColumn(table: "dbo.EntityApproval", name: "AppprovedById", newName: "ApprovedById");
            RenameIndex(table: "dbo.EntityApproval", name: "IX_AppprovedById", newName: "IX_ApprovedById");
            CreateTable(
                "dbo.Attachment",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(maxLength: 256),
                        FileId = c.Guid(),
                        ParentEntityId = c.Guid(),
                        EntityType = c.String(maxLength: 128),
                        CreatedBy = c.String(maxLength: 64),
                        CreatedOn = c.DateTime(),
                        LastModifiedBy = c.String(maxLength: 64),
                        LastModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileStorage", t => t.FileId)
                .Index(t => t.FileId)
                .Index(t => t.ParentEntityId)
                .Index(t => t.EntityType);
            
            AlterColumn("dbo.FileStorage", "Name", c => c.String(maxLength: 256));
            AlterColumn("dbo.FileStorage", "Extension", c => c.String(maxLength: 8));
            AlterColumn("dbo.CustomField", "LabelText", c => c.String(maxLength: 128));
            AlterColumn("dbo.CustomField", "MinValue", c => c.String(maxLength: 64));
            AlterColumn("dbo.CustomField", "MaxValue", c => c.String(maxLength: 64));
            AlterColumn("dbo.CustomFieldOption", "Code", c => c.String(maxLength: 16));
            AlterColumn("dbo.CustomFieldOption", "Description", c => c.String(maxLength: 256));
            AlterColumn("dbo.EntityMapping", "PropertyName", c => c.String(maxLength: 128));
            AlterColumn("dbo.SavedEntityMapping", "Description", c => c.String(maxLength: 256));
            AlterColumn("dbo.SavedEntityMappingField", "ColumnName", c => c.String(maxLength: 128));
            DropColumn("dbo.FileStorage", "LastModifiedBy");
            DropColumn("dbo.FileStorage", "LastModifiedOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FileStorage", "LastModifiedOn", c => c.DateTime());
            AddColumn("dbo.FileStorage", "LastModifiedBy", c => c.String(maxLength: 64));
            DropForeignKey("dbo.Attachment", "FileId", "dbo.FileStorage");
            DropIndex("dbo.Attachment", new[] { "EntityType" });
            DropIndex("dbo.Attachment", new[] { "ParentEntityId" });
            DropIndex("dbo.Attachment", new[] { "FileId" });
            AlterColumn("dbo.SavedEntityMappingField", "ColumnName", c => c.String());
            AlterColumn("dbo.SavedEntityMapping", "Description", c => c.String());
            AlterColumn("dbo.EntityMapping", "PropertyName", c => c.String());
            AlterColumn("dbo.CustomFieldOption", "Description", c => c.String());
            AlterColumn("dbo.CustomFieldOption", "Code", c => c.String());
            AlterColumn("dbo.CustomField", "MaxValue", c => c.String());
            AlterColumn("dbo.CustomField", "MinValue", c => c.String());
            AlterColumn("dbo.CustomField", "LabelText", c => c.String());
            AlterColumn("dbo.FileStorage", "Extension", c => c.String(maxLength: 256));
            AlterColumn("dbo.FileStorage", "Name", c => c.String());
            DropTable("dbo.Attachment");
            RenameIndex(table: "dbo.EntityApproval", name: "IX_ApprovedById", newName: "IX_AppprovedById");
            RenameColumn(table: "dbo.EntityApproval", name: "ApprovedById", newName: "AppprovedById");
            RenameTable(name: "dbo.SavedEntityMappingField", newName: "SavedEntityMappingFields");
            RenameTable(name: "dbo.SavedEntityMapping", newName: "SavedEntityMappings");
            RenameTable(name: "dbo.EntityMapping", newName: "EntityMappings");
        }
    }
}

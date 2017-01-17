namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCustomFields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomField",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        LabelText = c.String(),
                        MinValue = c.String(),
                        MaxValue = c.String(),
                        DecimalPlaces = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(),
                        Entity = c.Int(nullable: false),
                        FieldType = c.Int(nullable: false),
                        Answer_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomFieldData", t => t.Answer_Id)
                .Index(t => t.Answer_Id);
            
            CreateTable(
                "dbo.CustomFieldData",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CustomFieldId = c.Guid(nullable: false),
                        EntityType = c.Int(nullable: false),
                        ParentEntityId = c.Guid(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomFieldOption",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CustomFieldId = c.Guid(nullable: false),
                        Code = c.String(),
                        Description = c.String(),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomField", t => t.CustomFieldId, cascadeDelete: true)
                .Index(t => t.CustomFieldId);
            

        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Configuration",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ModuleType = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.MaritialStatus", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConstituentType", "Category", c => c.Int(nullable: false));
            AddColumn("dbo.ConstituentStatus", "IsRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.ConstituentStatus", "BaseStatus", c => c.Int(nullable: false));
            DropForeignKey("dbo.CustomFieldOption", "CustomFieldId", "dbo.CustomField");
            DropForeignKey("dbo.CustomField", "Answer_Id", "dbo.CustomFieldData");
            DropIndex("dbo.CustomFieldOption", new[] { "CustomFieldId" });
            DropIndex("dbo.CustomField", new[] { "Answer_Id" });
            DropColumn("dbo.ConstituentType", "BaseType");
            DropTable("dbo.CustomFieldOption");
            DropTable("dbo.CustomFieldData");
            DropTable("dbo.CustomField");
        }
    }
}

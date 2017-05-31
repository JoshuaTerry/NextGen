namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesToEntityMappings : DbMigration
    {
        public override void Up()
        {  
            DropColumn("dbo.EntityMappings", "PropertyValue");
            DropColumn("dbo.EntityMappings", "PropertyType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EntityMappings", "PropertyType", c => c.Int(nullable: false));
            AddColumn("dbo.EntityMappings", "PropertyValue", c => c.String());            
        }
    }
}

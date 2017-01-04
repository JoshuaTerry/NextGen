namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Prefix_v2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Prefix", "Abbreviation", "Code");
            RenameColumn("dbo.Prefix", "Description", "Name");
            AddColumn("dbo.Prefix", "LabelPrefix", c => c.String(maxLength: 128));
            AddColumn("dbo.Prefix", "LabelAbbreviation", c => c.String(maxLength: 128));
            AddColumn("dbo.Prefix", "Salutation", c => c.String(maxLength: 128));
            AddColumn("dbo.Prefix", "ShowOnline", c => c.Boolean(nullable: false));
            DropColumn("dbo.Denomination", "Affiliation");
            DropColumn("dbo.Denomination", "Religion");
            AddColumn("dbo.Denomination", "Affiliation", c => c.Int(nullable: false));
            AddColumn("dbo.Denomination", "Religion", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Denomination", "Affiliation");
            DropColumn("dbo.Denomination", "Religion");
            AddColumn("dbo.Denomination", "Religion", c => c.String(maxLength: 128));
            AddColumn("dbo.Denomination", "Affiliation", c => c.String(maxLength: 128));
            DropColumn("dbo.Prefix", "ShowOnline");
            DropColumn("dbo.Prefix", "Salutation");
            DropColumn("dbo.Prefix", "LabelAbbreviation");
            DropColumn("dbo.Prefix", "LabelPrefix");
            RenameColumn("dbo.Prefix", "Code", "Abbreviation");
            RenameColumn("dbo.Prefix", "Name", "Description");
        }
    }
}

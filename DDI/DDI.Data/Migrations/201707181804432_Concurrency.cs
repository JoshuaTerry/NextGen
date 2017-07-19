namespace DDI.Data.Migrations.Common
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Concurrency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Abbreviation", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.City", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.CityName", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.County", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.State", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Country", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Zip", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.ZipBranch", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.ZipStreet", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.ZipPlus4", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterColumn("dbo.County", "Description", c => c.String(maxLength: 128));
            AlterColumn("dbo.State", "Description", c => c.String(maxLength: 128));
            AlterColumn("dbo.Country", "Description", c => c.String(maxLength: 128));
            AlterColumn("dbo.Country", "StateName", c => c.String(maxLength: 20));
            AlterColumn("dbo.Country", "PostalCodeFormat", c => c.String(maxLength: 128));
            AlterColumn("dbo.Country", "AddressFormat", c => c.String(maxLength: 128));
            AlterColumn("dbo.ZipBranch", "Description", c => c.String(maxLength: 128));
            AlterColumn("dbo.ZipStreet", "Street", c => c.String(maxLength: 40));
            AlterColumn("dbo.Thesaurus", "Expansion", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Thesaurus", "Expansion", c => c.String());
            AlterColumn("dbo.ZipStreet", "Street", c => c.String());
            AlterColumn("dbo.ZipBranch", "Description", c => c.String());
            AlterColumn("dbo.Country", "AddressFormat", c => c.String());
            AlterColumn("dbo.Country", "PostalCodeFormat", c => c.String());
            AlterColumn("dbo.Country", "StateName", c => c.String());
            AlterColumn("dbo.Country", "Description", c => c.String());
            AlterColumn("dbo.State", "Description", c => c.String());
            AlterColumn("dbo.County", "Description", c => c.String());
            DropColumn("dbo.ZipPlus4", "RowVersion");
            DropColumn("dbo.ZipStreet", "RowVersion");
            DropColumn("dbo.ZipBranch", "RowVersion");
            DropColumn("dbo.Zip", "RowVersion");
            DropColumn("dbo.Country", "RowVersion");
            DropColumn("dbo.State", "RowVersion");
            DropColumn("dbo.County", "RowVersion");
            DropColumn("dbo.CityName", "RowVersion");
            DropColumn("dbo.City", "RowVersion");
            DropColumn("dbo.Abbreviation", "RowVersion");
        }
    }
}

namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Constituent_v3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Address", "StateId", "dbo.State");
            DropForeignKey("dbo.Address", "Constituent_Id", "dbo.Constituent");
            DropIndex("dbo.Address", new[] { "StateId" });
            DropIndex("dbo.Address", new[] { "Constituent_Id" });
            CreateTable(
                "dbo.AddressType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        IsActive = c.Boolean(nullable: false),
                        Code = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Address", "StreetAddress", c => c.String());
            AddColumn("dbo.Address", "CountryId", c => c.Guid());
            AddColumn("dbo.Address", "CountyId", c => c.Guid());
            AddColumn("dbo.Address", "PostalCode", c => c.String());
            AddColumn("dbo.Address", "LegacyKey", c => c.Int(nullable: false));
            AddColumn("dbo.ConstituentAddress", "Comment", c => c.String(maxLength: 255));
            AddColumn("dbo.ConstituentAddress", "StartDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.ConstituentAddress", "EndDate", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.ConstituentAddress", "StartDay", c => c.Int(nullable: false));
            AddColumn("dbo.ConstituentAddress", "EndDay", c => c.Int(nullable: false));
            AddColumn("dbo.ConstituentAddress", "ResidentType", c => c.Int(nullable: false));
            AddColumn("dbo.ConstituentAddress", "AddressTypeId", c => c.Guid());
            AddColumn("dbo.ConstituentAddress", "DuplicateKey", c => c.String(maxLength: 128));
            AlterColumn("dbo.ContactInfo", "Comment", c => c.String(maxLength: 255));
            CreateIndex("dbo.ConstituentAddress", "AddressTypeId");
            CreateIndex("dbo.ConstituentAddress", "DuplicateKey");
            AddForeignKey("dbo.ConstituentAddress", "AddressTypeId", "dbo.AddressType", "Id");
            DropColumn("dbo.Address", "Line1");
            DropColumn("dbo.Address", "Line2");
            DropColumn("dbo.Address", "Zip");
            DropColumn("dbo.Address", "Constituent_Id");
            DropColumn("dbo.ContactInfo", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContactInfo", "Name", c => c.String());
            AddColumn("dbo.Address", "Constituent_Id", c => c.Guid());
            AddColumn("dbo.Address", "Zip", c => c.String());
            AddColumn("dbo.Address", "Line2", c => c.String());
            AddColumn("dbo.Address", "Line1", c => c.String());
            DropForeignKey("dbo.ConstituentAddress", "AddressTypeId", "dbo.AddressType");
            DropIndex("dbo.ConstituentAddress", new[] { "DuplicateKey" });
            DropIndex("dbo.ConstituentAddress", new[] { "AddressTypeId" });
            AlterColumn("dbo.ContactInfo", "Comment", c => c.String());
            DropColumn("dbo.ConstituentAddress", "DuplicateKey");
            DropColumn("dbo.ConstituentAddress", "AddressTypeId");
            DropColumn("dbo.ConstituentAddress", "ResidentType");
            DropColumn("dbo.ConstituentAddress", "EndDay");
            DropColumn("dbo.ConstituentAddress", "StartDay");
            DropColumn("dbo.ConstituentAddress", "EndDate");
            DropColumn("dbo.ConstituentAddress", "StartDate");
            DropColumn("dbo.ConstituentAddress", "Comment");
            DropColumn("dbo.Address", "LegacyKey");
            DropColumn("dbo.Address", "PostalCode");
            DropColumn("dbo.Address", "CountyId");
            DropColumn("dbo.Address", "CountryId");
            DropColumn("dbo.Address", "StreetAddress");
            DropTable("dbo.AddressType");
            CreateIndex("dbo.Address", "Constituent_Id");
            CreateIndex("dbo.Address", "StateId");
            AddForeignKey("dbo.Address", "Constituent_Id", "dbo.Constituent", "Id");
            AddForeignKey("dbo.Address", "StateId", "dbo.State", "Id");
        }
    }
}

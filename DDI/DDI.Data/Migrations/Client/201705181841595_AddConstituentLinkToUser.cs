namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConstituentLinkToUser : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ConstituentPaymentMethods", newName: "PaymentMethodConstituents");
            DropPrimaryKey("dbo.PaymentMethodConstituents");
            AddColumn("dbo.Users", "ConstituentId", c => c.Guid());
            AddPrimaryKey("dbo.PaymentMethodConstituents", new[] { "PaymentMethod_Id", "Constituent_Id" });
            CreateIndex("dbo.Users", "ConstituentId");
            AddForeignKey("dbo.Users", "ConstituentId", "dbo.Constituent", "Id");
            DropColumn("dbo.Constituent", "BirthDate");
            DropColumn("dbo.Constituent", "BirthYear");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Constituent", "BirthYear", c => c.Int());
            AddColumn("dbo.Constituent", "BirthDate", c => c.DateTime(storeType: "date"));
            DropForeignKey("dbo.Users", "ConstituentId", "dbo.Constituent");
            DropIndex("dbo.Users", new[] { "ConstituentId" });
            DropPrimaryKey("dbo.PaymentMethodConstituents");
            DropColumn("dbo.Users", "ConstituentId");
            AddPrimaryKey("dbo.PaymentMethodConstituents", new[] { "Constituent_Id", "PaymentMethod_Id" });
            RenameTable(name: "dbo.PaymentMethodConstituents", newName: "ConstituentPaymentMethods");
        }
    }
}

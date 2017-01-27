namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentPreferences : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentPreference", "ConstituentId", "dbo.Constituent");
            DropIndex("dbo.PaymentPreference", new[] { "ConstituentId" });
            DropTable("dbo.PaymentPreference");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PaymentPreference",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ConstituentId = c.Guid(),
                        Name = c.String(maxLength: 128),
                        ABANumber = c.Int(nullable: false),
                        AccountNumber = c.String(maxLength: 128),
                        AccountType = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.PaymentPreference", "ConstituentId");
            AddForeignKey("dbo.PaymentPreference", "ConstituentId", "dbo.Constituent", "Id");
        }
    }
}

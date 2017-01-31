namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentMethod_v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentMethod", "AccountType", c => c.Int());
            DropColumn("dbo.PaymentMethod", "TranCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentMethod", "TranCode", c => c.Int());
            DropColumn("dbo.PaymentMethod", "AccountType");
        }
    }
}

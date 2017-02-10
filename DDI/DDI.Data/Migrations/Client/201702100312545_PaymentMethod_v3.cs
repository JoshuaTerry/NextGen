namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentMethod_v3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentMethod", "Category", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentMethod", "Category");
        }
    }
}

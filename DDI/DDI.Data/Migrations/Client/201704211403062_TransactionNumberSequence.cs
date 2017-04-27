namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    using Extensions;
    using Statics;

    public partial class TransactionNumberSequence : DbMigration
    {
        public override void Up()
        {
            this.CreateSequence(Sequences.TransactionNumber, "bigint");
        }
        
        public override void Down()
        {
            this.DropSequence(Sequences.TransactionNumber);
        }
    }
}

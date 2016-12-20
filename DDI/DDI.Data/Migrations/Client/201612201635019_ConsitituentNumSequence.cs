namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConsitituentNumSequence : DbMigration
    {
        public override void Up()
        {
            this.Sql($"CREATE SEQUENCE {DomainContext.ConstituentNumberSequence} AS int START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 999999999 NO CYCLE;");
        }
        
        public override void Down()
        {
            this.Sql($"DROP SEQUENCE {DomainContext.ConstituentNumberSequence}");
        }
    }
}

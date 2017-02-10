namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBirthYearToConstituent : DbMigration
    {
        public override void Up()
        {
 
            AddColumn("dbo.Constituent", "BirthYear", c => c.Int());
       
        }
        
        public override void Down()
        {
            
            DropColumn("dbo.Constituent", "BirthYear");
            
        }
    }
}

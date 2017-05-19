namespace DDI.Data.Migrations.Client
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingUniqueIndexToUserName : DbMigration
    {
        public override void Up()
        {             
            AlterColumn("dbo.Users", "UserName", c => c.String(maxLength: 256));
            CreateIndex("dbo.Users", "UserName", unique: true); 
        }
        
        public override void Down()
        {            
            DropIndex("dbo.Users", new[] { "UserName" });
            AlterColumn("dbo.Users", "UserName", c => c.String());
           
        }
    }
}

namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedModelCase : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Cocktails", new[] { "bartenderId" });
            CreateIndex("dbo.Cocktails", "BartenderId");
            DropColumn("dbo.Bartenders", "lastDrink");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bartenders", "lastDrink", c => c.DateTime(nullable: false));
            DropIndex("dbo.Cocktails", new[] { "BartenderId" });
            CreateIndex("dbo.Cocktails", "bartenderId");
        }
    }
}

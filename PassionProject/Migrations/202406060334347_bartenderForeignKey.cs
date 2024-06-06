namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bartenderForeignKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cocktails", "bartenderId", c => c.Int(nullable: false));
            CreateIndex("dbo.Cocktails", "bartenderId");
            AddForeignKey("dbo.Cocktails", "bartenderId", "dbo.Bartenders", "bartenderId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cocktails", "bartenderId", "dbo.Bartenders");
            DropIndex("dbo.Cocktails", new[] { "bartenderId" });
            DropColumn("dbo.Cocktails", "bartenderId");
        }
    }
}

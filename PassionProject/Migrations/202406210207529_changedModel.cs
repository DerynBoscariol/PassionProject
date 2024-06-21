namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Cocktails", "datePosted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cocktails", "datePosted", c => c.DateTime(nullable: false));
        }
    }
}

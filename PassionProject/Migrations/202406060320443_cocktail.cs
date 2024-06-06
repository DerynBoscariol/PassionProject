namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cocktail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cocktails",
                c => new
                    {
                        drinkId = c.Int(nullable: false, identity: true),
                        drinkName = c.String(),
                        drinkType = c.String(),
                        drinkRecipe = c.String(),
                        liqIn = c.String(),
                        mixIn = c.String(),
                        datePosted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.drinkId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cocktails");
        }
    }
}

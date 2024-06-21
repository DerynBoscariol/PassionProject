namespace PassionProject.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class bartenderMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bartenders",
                c => new
                {
                    bartenderId = c.Int(nullable: false, identity: true),
                    firstName = c.String(),
                    lastName = c.String(),
                    email = c.String(),
                    numDrinks = c.Int(nullable: false),
                    lastDrink = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.bartenderId);

        }

        public override void Down()
        {
            DropTable("dbo.Bartenders");
        }
    }
}

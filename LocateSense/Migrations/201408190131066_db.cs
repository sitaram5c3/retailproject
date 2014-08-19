namespace LocateSense.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        email = c.String(),
                        telephone = c.String(),
                        guid = c.String(),
                        password = c.String(),
                        isLive = c.Boolean(nullable: false),
                        isLoggedOn = c.Boolean(nullable: false),
                        lastLoggedOn = c.DateTime(nullable: false),
                        level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.offers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        startDateTime = c.DateTime(),
                        endDateTime = c.DateTime(),
                        title = c.String(),
                        description = c.String(),
                        strapLine = c.String(),
                        price = c.Decimal(precision: 18, scale: 2),
                        productId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        manufacturer = c.String(),
                        productName = c.String(),
                        imageURL = c.String(),
                        imageInstallationURL = c.String(),
                        availableStock = c.Int(nullable: false),
                        numberOfVisits = c.Int(nullable: false),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UUID = c.String(),
                        category = c.String(),
                        productOwner = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.locates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        vistDateTime = c.DateTime(nullable: false),
                        userId = c.Int(nullable: false),
                        beaconId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.locates");
            DropTable("dbo.products");
            DropTable("dbo.offers");
            DropTable("dbo.users");
        }
    }
}

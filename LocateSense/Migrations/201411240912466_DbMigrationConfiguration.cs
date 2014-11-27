namespace LocateSense.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigrationConfiguration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.offers", "enalbed", c => c.Int());
            AddColumn("dbo.offers", "deleted", c => c.Int());
            AddColumn("dbo.offers", "alwaysEnabledOffer", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.offers", "alwaysEnabledOffer");
            DropColumn("dbo.offers", "deleted");
            DropColumn("dbo.offers", "enalbed");
        }
    }
}

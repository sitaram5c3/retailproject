namespace LocateSense.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigrationConfiguration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.offers", "enalbed", c => c.Boolean());
            AlterColumn("dbo.offers", "deleted", c => c.Boolean());
            AlterColumn("dbo.offers", "alwaysEnabledOffer", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.offers", "alwaysEnabledOffer", c => c.Int());
            AlterColumn("dbo.offers", "deleted", c => c.Int());
            AlterColumn("dbo.offers", "enalbed", c => c.Int());
        }
    }
}

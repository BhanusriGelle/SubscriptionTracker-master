namespace SubscriptionTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class minor_changes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblServices", "LogoUrl", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblServices", "LogoUrl", c => c.String(nullable: false));
        }
    }
}

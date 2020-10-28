namespace SubscriptionTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_EndDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblServices", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tblServices", "BillingTerm", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblServices", "BillingTerm", c => c.String(nullable: false));
            DropColumn("dbo.tblServices", "EndDate");
        }
    }
}

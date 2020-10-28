namespace SubscriptionTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblServices",
                c => new
                    {
                        ServiceId = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(nullable: false),
                        LogoUrl = c.String(nullable: false),
                        PlanStatus = c.String(nullable: false),
                        BillingTerm = c.String(nullable: false),
                        Pricing = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartDate = c.DateTime(nullable: false),
                        ServiceType = c.String(nullable: false),
                        User_EmailId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ServiceId)
                .ForeignKey("dbo.tblUsers", t => t.User_EmailId, cascadeDelete: true)
                .Index(t => t.User_EmailId);
            
            CreateTable(
                "dbo.tblUsers",
                c => new
                    {
                        EmailId = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        PhoneNo = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EmailId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblServices", "User_EmailId", "dbo.tblUsers");
            DropIndex("dbo.tblServices", new[] { "User_EmailId" });
            DropTable("dbo.tblUsers");
            DropTable("dbo.tblServices");
        }
    }
}

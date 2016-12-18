namespace Superior.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserLoginMonitor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserLoginMonitors",
                c => new
                    {
                        UserLoginMonitorId = c.Guid(nullable: false),
                        LoginAttemptCount = c.Short(nullable: false),
                        UtcDateTimeLastLoginAttempt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserLoginMonitorId)
                .ForeignKey("dbo.Users", t => t.UserLoginMonitorId)
                .Index(t => t.UserLoginMonitorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserLoginMonitors", "UserLoginMonitorId", "dbo.Users");
            DropIndex("dbo.UserLoginMonitors", new[] { "UserLoginMonitorId" });
            DropTable("dbo.UserLoginMonitors");
        }
    }
}

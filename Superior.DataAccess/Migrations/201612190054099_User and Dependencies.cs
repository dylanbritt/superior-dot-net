namespace Superior.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserandDependencies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCredentials",
                c => new
                    {
                        UserCredentialId = c.Guid(nullable: false),
                        EncryptedPassword = c.Binary(nullable: false, maxLength: 256),
                        Salt = c.Binary(nullable: false, maxLength: 64),
                        UtcDateTimeCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserCredentialId)
                .ForeignKey("dbo.Users", t => t.UserCredentialId)
                .Index(t => t.UserCredentialId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 64),
                        UtcDateTimeCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true);
            
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
            DropForeignKey("dbo.UserCredentials", "UserCredentialId", "dbo.Users");
            DropForeignKey("dbo.UserLoginMonitors", "UserLoginMonitorId", "dbo.Users");
            DropIndex("dbo.UserLoginMonitors", new[] { "UserLoginMonitorId" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.UserCredentials", new[] { "UserCredentialId" });
            DropTable("dbo.UserLoginMonitors");
            DropTable("dbo.Users");
            DropTable("dbo.UserCredentials");
        }
    }
}

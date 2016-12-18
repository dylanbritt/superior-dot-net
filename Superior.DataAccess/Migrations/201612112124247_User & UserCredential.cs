namespace Superior.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserUserCredential : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserCredentials",
                c => new
                    {
                        UserCredentialId = c.Guid(nullable: false),
                        EncryptedPassword = c.Binary(nullable: false),
                        Salt = c.Binary(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserCredentials", "UserCredentialId", "dbo.Users");
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.UserCredentials", new[] { "UserCredentialId" });
            DropTable("dbo.Users");
            DropTable("dbo.UserCredentials");
        }
    }
}

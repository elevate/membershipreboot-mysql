using System.Data.Entity.Migrations;

namespace Elevate.Accounts.Web.Data.Migrations
{
    public partial class v7_InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Groups",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false),
                        Tenant = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 100),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "GroupChilds",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        ChildGroupID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("Groups", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "UserAccounts",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ID = c.Guid(nullable: false),
                        Tenant = c.String(nullable: false, maxLength: 50),
                        Username = c.String(nullable: false, maxLength: 254),
                        Created = c.DateTime(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        IsAccountClosed = c.Boolean(nullable: false),
                        AccountClosed = c.DateTime(),
                        IsLoginAllowed = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(),
                        LastFailedLogin = c.DateTime(),
                        FailedLoginCount = c.Int(nullable: false),
                        PasswordChanged = c.DateTime(),
                        RequiresPasswordReset = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 254),
                        IsAccountVerified = c.Boolean(nullable: false),
                        LastFailedPasswordReset = c.DateTime(),
                        FailedPasswordResetCount = c.Int(nullable: false),
                        MobileCode = c.String(maxLength: 100),
                        MobileCodeSent = c.DateTime(),
                        MobilePhoneNumber = c.String(maxLength: 20),
                        MobilePhoneNumberChanged = c.DateTime(),
                        AccountTwoFactorAuthMode = c.Int(nullable: false),
                        CurrentTwoFactorAuthStatus = c.Int(nullable: false),
                        VerificationKey = c.String(maxLength: 100),
                        VerificationPurpose = c.Int(),
                        VerificationKeySent = c.DateTime(),
                        VerificationStorage = c.String(maxLength: 100),
                        HashedPassword = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "UserClaims",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Type = c.String(nullable: false, maxLength: 150),
                        Value = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "LinkedAccountClaims",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        ProviderName = c.String(nullable: false, maxLength: 30),
                        ProviderAccountID = c.String(nullable: false, maxLength: 100),
                        Type = c.String(nullable: false, maxLength: 150),
                        Value = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "LinkedAccounts",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        ProviderName = c.String(nullable: false, maxLength: 30),
                        ProviderAccountID = c.String(nullable: false, maxLength: 100),
                        LastLogin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "PasswordResetSecrets",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        PasswordResetSecretID = c.Guid(nullable: false),
                        Question = c.String(nullable: false, maxLength: 150),
                        Answer = c.String(nullable: false, maxLength: 150),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "TwoFactorAuthTokens",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Token = c.String(nullable: false, maxLength: 100),
                        Issued = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
            CreateTable(
                "UserCertificates",
                c => new
                    {
                        Key = c.Int(nullable: false, identity: true),
                        ParentKey = c.Int(nullable: false),
                        Thumbprint = c.String(nullable: false, maxLength: 150),
                        Subject = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Key)
                .ForeignKey("UserAccounts", t => t.ParentKey, cascadeDelete: true)
                .Index(t => t.ParentKey);
            
        }
        
        public override void Down()
        {
            DropForeignKey("UserCertificates", "ParentKey", "UserAccounts");
            DropForeignKey("TwoFactorAuthTokens", "ParentKey", "UserAccounts");
            DropForeignKey("PasswordResetSecrets", "ParentKey", "UserAccounts");
            DropForeignKey("LinkedAccounts", "ParentKey", "UserAccounts");
            DropForeignKey("LinkedAccountClaims", "ParentKey", "UserAccounts");
            DropForeignKey("UserClaims", "ParentKey", "UserAccounts");
            DropForeignKey("GroupChilds", "ParentKey", "Groups");
            DropIndex("UserCertificates", new[] { "ParentKey" });
            DropIndex("TwoFactorAuthTokens", new[] { "ParentKey" });
            DropIndex("PasswordResetSecrets", new[] { "ParentKey" });
            DropIndex("LinkedAccounts", new[] { "ParentKey" });
            DropIndex("LinkedAccountClaims", new[] { "ParentKey" });
            DropIndex("UserClaims", new[] { "ParentKey" });
            DropIndex("GroupChilds", new[] { "ParentKey" });
            DropTable("UserCertificates");
            DropTable("TwoFactorAuthTokens");
            DropTable("PasswordResetSecrets");
            DropTable("LinkedAccounts");
            DropTable("LinkedAccountClaims");
            DropTable("UserClaims");
            DropTable("UserAccounts");
            DropTable("GroupChilds");
            DropTable("Groups");
        }
    }
}

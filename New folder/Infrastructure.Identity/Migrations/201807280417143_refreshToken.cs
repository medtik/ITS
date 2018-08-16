namespace Infrastructure.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refreshToken : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 255),
                        Secret = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        ApplicationType = c.Int(),
                        Active = c.Boolean(),
                        RefreshTokenLifeTime = c.Int(),
                        AllowedOrigin = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RefreshToken",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 255),
                        Subject = c.String(nullable: false, maxLength: 255),
                        ClientId = c.String(nullable: false, maxLength: 255),
                        IssuedUtc = c.DateTime(),
                        ExpiresUtc = c.DateTime(),
                        ProtectedTicket = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
        }
    }
}

namespace Infrastructure.Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _123 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RefreshToken", "ProtectedTicket", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RefreshToken", "ProtectedTicket", c => c.String(nullable: false, maxLength: 255));
        }
    }
}

namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8_8_2018 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "MobileToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "MobileToken");
        }
    }
}

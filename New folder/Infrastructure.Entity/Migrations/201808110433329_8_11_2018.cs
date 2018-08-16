namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8_11_2018 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "TotalTimeStay", c => c.Int());
            AddColumn("dbo.Locations", "TotalStayCount", c => c.Int());
            AddColumn("dbo.Locations", "LocationRadius", c => c.Double());
            AlterColumn("dbo.Locations", "Category", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Locations", "Category", c => c.String(maxLength: 255));
            DropColumn("dbo.Locations", "LocationRadius");
            DropColumn("dbo.Locations", "TotalStayCount");
            DropColumn("dbo.Locations", "TotalTimeStay");
        }
    }
}

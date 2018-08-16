namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8_5_2018_3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LocationSuggestion", "PlanId", "dbo.Plans");
            DropColumn("dbo.LocationSuggestion", "PlanId");
        }
        
        public override void Down()
        {
        }
    }
}

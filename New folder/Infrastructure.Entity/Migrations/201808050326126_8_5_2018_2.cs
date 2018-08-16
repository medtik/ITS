namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8_5_2018_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LocationSuggestion", "Id", "dbo.Plans");
            AddForeignKey("dbo.LocationSuggestion", "Id", "dbo.Plans", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocationSuggestion", "Id", "dbo.Plans");
            AddForeignKey("dbo.LocationSuggestion", "Id", "dbo.Plans", "Id", cascadeDelete: true);
        }
    }
}

namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8_5_2018 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LocationSuggestion", "Id", "dbo.Locations");
            DropIndex("dbo.LocationSuggestion", new[] { "PlanId" });
            AddColumn("dbo.LocationSuggestion", "LocationId", c => c.Int(nullable: false));
            CreateIndex("dbo.LocationSuggestion", "LocationId");
            AddForeignKey("dbo.LocationSuggestion", "LocationId", "dbo.Locations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocationSuggestion", "LocationId", "dbo.Locations");
            DropIndex("dbo.LocationSuggestion", new[] { "LocationId" });
            DropColumn("dbo.LocationSuggestion", "LocationId");
            RenameColumn(table: "dbo.LocationSuggestion", name: "Id", newName: "PlanId");
            AddColumn("dbo.LocationSuggestion", "Id", c => c.Int(nullable: false));
            CreateIndex("dbo.LocationSuggestion", "PlanId");
            AddForeignKey("dbo.LocationSuggestion", "Id", "dbo.Locations", "Id");
        }
    }
}

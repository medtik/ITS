namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13_8_2018 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LocationSuggestion", "LocationId", "dbo.Locations");
            DropIndex("dbo.LocationSuggestion", new[] { "LocationId" });
            CreateTable(
                "dbo.MappingLocationSuggestionLocations",
                c => new
                    {
                        LocationId = c.Int(nullable: false),
                        SuggestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LocationId, t.SuggestionId })
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.LocationSuggestion", t => t.SuggestionId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.SuggestionId);
            
            AddColumn("dbo.LocationSuggestion", "PlanDay", c => c.Int());
            DropColumn("dbo.LocationSuggestion", "LocationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LocationSuggestion", "LocationId", c => c.Int(nullable: false));
            DropForeignKey("dbo.MappingLocationSuggestionLocations", "SuggestionId", "dbo.LocationSuggestion");
            DropForeignKey("dbo.MappingLocationSuggestionLocations", "LocationId", "dbo.Locations");
            DropIndex("dbo.MappingLocationSuggestionLocations", new[] { "SuggestionId" });
            DropIndex("dbo.MappingLocationSuggestionLocations", new[] { "LocationId" });
            DropColumn("dbo.LocationSuggestion", "PlanDay");
            DropTable("dbo.MappingLocationSuggestionLocations");
            CreateIndex("dbo.LocationSuggestion", "LocationId");
            AddForeignKey("dbo.LocationSuggestion", "LocationId", "dbo.Locations", "Id", cascadeDelete: true);
        }
    }
}

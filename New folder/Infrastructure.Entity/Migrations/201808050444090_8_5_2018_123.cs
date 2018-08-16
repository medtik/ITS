namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8_5_2018_123 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LocationSuggestion", "Id", "dbo.Plans");
            DropIndex("dbo.LocationSuggestion", new[] { "Id" });
            DropPrimaryKey("dbo.LocationSuggestion");
            AddColumn("dbo.LocationSuggestion", "PlanId", c => c.Int(nullable: false));
            AlterColumn("dbo.LocationSuggestion", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.LocationSuggestion", "Id");
            CreateIndex("dbo.LocationSuggestion", "PlanId");
            AddForeignKey("dbo.LocationSuggestion", "PlanId", "dbo.Plans", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocationSuggestion", "PlanId", "dbo.Plans");
            DropIndex("dbo.LocationSuggestion", new[] { "PlanId" });
            DropPrimaryKey("dbo.LocationSuggestion");
            AlterColumn("dbo.LocationSuggestion", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.LocationSuggestion", "PlanId");
            AddPrimaryKey("dbo.LocationSuggestion", "Id");
            CreateIndex("dbo.LocationSuggestion", "Id");
            AddForeignKey("dbo.LocationSuggestion", "Id", "dbo.Plans", "Id");
        }
    }
}

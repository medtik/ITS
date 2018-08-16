namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editDatabase2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Plans", "Id", "dbo.LocationSuggestion");
            DropForeignKey("dbo.Notes", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.PlanLocations", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.UserPlanVote", "VotePlanId", "dbo.Plans");
            DropIndex("dbo.Plans", new[] { "Id" });
            DropPrimaryKey("dbo.Plans");
            AlterColumn("dbo.Plans", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Plans", "Id");
            CreateIndex("dbo.LocationSuggestion", "PlanId");
            AddForeignKey("dbo.LocationSuggestion", "PlanId", "dbo.Plans", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Notes", "PlanId", "dbo.Plans", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PlanLocations", "PlanId", "dbo.Plans", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserPlanVote", "VotePlanId", "dbo.Plans", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPlanVote", "VotePlanId", "dbo.Plans");
            DropForeignKey("dbo.PlanLocations", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.Notes", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.LocationSuggestion", "PlanId", "dbo.Plans");
            DropIndex("dbo.LocationSuggestion", new[] { "PlanId" });
            DropPrimaryKey("dbo.Plans");
            AlterColumn("dbo.Plans", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Plans", "Id");
            CreateIndex("dbo.Plans", "Id");
            AddForeignKey("dbo.UserPlanVote", "VotePlanId", "dbo.Plans", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PlanLocations", "PlanId", "dbo.Plans", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Notes", "PlanId", "dbo.Plans", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Plans", "Id", "dbo.LocationSuggestion", "Id");
        }
    }
}

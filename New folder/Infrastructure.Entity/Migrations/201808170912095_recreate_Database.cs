namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recreate_Database : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Categories = c.String(maxLength: 510),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Description = c.String(),
                        Country = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Address = c.String(maxLength: 510),
                        Description = c.String(maxLength: 510),
                        Longitude = c.Double(),
                        Latitude = c.Double(),
                        Website = c.String(maxLength: 510),
                        PhoneNumber = c.String(maxLength: 255),
                        EmailAddress = c.String(maxLength: 510),
                        IsVerified = c.Boolean(),
                        IsClosed = c.Boolean(),
                        IsDelete = c.Boolean(),
                        Category = c.String(),
                        AreaId = c.Int(nullable: false),
                        TotalTimeStay = c.Int(),
                        TotalStayCount = c.Int(),
                        LocationRadius = c.Double(),
                        CreatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Creators", t => t.CreatorId)
                .ForeignKey("dbo.Areas", t => t.AreaId, cascadeDelete: true)
                .Index(t => t.AreaId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.BusinessHours",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.String(),
                        OpenTime = c.Time(precision: 7),
                        CloseTime = c.Time(precision: 7),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.ChangeRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Address = c.String(maxLength: 510),
                        Description = c.String(),
                        Website = c.String(),
                        PhoneNumber = c.String(maxLength: 255),
                        EmailAddress = c.String(maxLength: 510),
                        BusinessHours = c.String(),
                        Tags = c.String(),
                        Status = c.Int(),
                        LocationId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(maxLength: 255),
                        Birthdate = c.DateTimeOffset(precision: 7),
                        Avatar = c.String(),
                        Address = c.String(),
                        MobileToken = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClaimOwnerRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Status = c.Int(),
                        LocationId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GroupInvitations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Status = c.Int(),
                        UserId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        CreatorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Creators", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Plans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        StartDate = c.DateTimeOffset(precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        IsPublic = c.Boolean(),
                        GroupId = c.Int(),
                        CreatorId = c.Int(nullable: false),
                        MemberId = c.Int(),
                        AreaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.AreaId)
                .ForeignKey("dbo.Creators", t => t.CreatorId)
                .ForeignKey("dbo.Members", t => t.MemberId)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .Index(t => t.GroupId)
                .Index(t => t.CreatorId)
                .Index(t => t.MemberId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "dbo.LocationSuggestion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(),
                        Comment = c.String(),
                        UserId = c.Int(nullable: false),
                        PlanDay = c.Int(),
                        PlanId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plans", t => t.PlanId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PlanId);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        ReviewId = c.Int(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reviews", t => t.ReviewId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ReviewId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AreaPhoto",
                c => new
                    {
                        AreaId = c.Int(nullable: false),
                        PhotoId = c.Int(nullable: false),
                        IsPrimary = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.AreaId, t.PhotoId })
                .ForeignKey("dbo.Areas", t => t.AreaId, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.PhotoId, cascadeDelete: true)
                .Index(t => t.AreaId)
                .Index(t => t.PhotoId);
            
            CreateTable(
                "dbo.LocationPhoto",
                c => new
                    {
                        LocationId = c.Int(nullable: false),
                        PhotoId = c.Int(nullable: false),
                        IsPrimary = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.LocationId, t.PhotoId })
                .ForeignKey("dbo.Photos", t => t.PhotoId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.PhotoId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 510),
                        Rating = c.Single(),
                        Description = c.String(),
                        CreatorId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Creators", t => t.CreatorId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .Index(t => t.CreatorId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        UserId = c.Int(nullable: false),
                        ReviewId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reviews", t => t.ReviewId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ReviewId);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 255),
                        Content = c.String(),
                        PlanDay = c.Int(),
                        Index = c.Int(),
                        Done = c.Boolean(),
                        PlanId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plans", t => t.PlanId, cascadeDelete: true)
                .Index(t => t.PlanId);
            
            CreateTable(
                "dbo.PlanLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlanDay = c.Int(),
                        Index = c.Int(),
                        Comment = c.String(),
                        Done = c.Boolean(),
                        PlanId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plans", t => t.PlanId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.PlanId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        Categories = c.String(maxLength: 510),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MemberGroup",
                c => new
                    {
                        MemberId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MemberId, t.GroupId })
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.MemberId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.UserPlanVote",
                c => new
                    {
                        VoterId = c.Int(nullable: false),
                        VotePlanId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VoterId, t.VotePlanId })
                .ForeignKey("dbo.Users", t => t.VoterId, cascadeDelete: true)
                .ForeignKey("dbo.Plans", t => t.VotePlanId, cascadeDelete: true)
                .Index(t => t.VoterId)
                .Index(t => t.VotePlanId);
            
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
            
            CreateTable(
                "dbo.LocationTag",
                c => new
                    {
                        LocationId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LocationId, t.TagId })
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.QuestionArea",
                c => new
                    {
                        QuestionId = c.Int(nullable: false),
                        AreaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionId, t.AreaId })
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Areas", t => t.AreaId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.AreaId);
            
            CreateTable(
                "dbo.AnswerTag",
                c => new
                    {
                        AnswerId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AnswerId, t.TagId })
                .ForeignKey("dbo.Answers", t => t.AnswerId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.AnswerId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Creators",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Creators", "Id", "dbo.Members");
            DropForeignKey("dbo.Members", "Id", "dbo.Users");
            DropForeignKey("dbo.AnswerTag", "TagId", "dbo.Tags");
            DropForeignKey("dbo.AnswerTag", "AnswerId", "dbo.Answers");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.QuestionArea", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.QuestionArea", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Locations", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.LocationTag", "TagId", "dbo.Tags");
            DropForeignKey("dbo.LocationTag", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Reviews", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.PlanLocations", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.LocationPhoto", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.MappingLocationSuggestionLocations", "SuggestionId", "dbo.LocationSuggestion");
            DropForeignKey("dbo.MappingLocationSuggestionLocations", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.ClaimOwnerRequests", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.ChangeRequests", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.UserPlanVote", "VotePlanId", "dbo.Plans");
            DropForeignKey("dbo.UserPlanVote", "VoterId", "dbo.Users");
            DropForeignKey("dbo.Reports", "UserId", "dbo.Users");
            DropForeignKey("dbo.Photos", "UserId", "dbo.Users");
            DropForeignKey("dbo.LocationSuggestion", "UserId", "dbo.Users");
            DropForeignKey("dbo.GroupInvitations", "UserId", "dbo.Users");
            DropForeignKey("dbo.Plans", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.GroupInvitations", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Reviews", "CreatorId", "dbo.Creators");
            DropForeignKey("dbo.Locations", "CreatorId", "dbo.Creators");
            DropForeignKey("dbo.PlanLocations", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.Notes", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.Plans", "MemberId", "dbo.Members");
            DropForeignKey("dbo.Reports", "ReviewId", "dbo.Reviews");
            DropForeignKey("dbo.Photos", "ReviewId", "dbo.Reviews");
            DropForeignKey("dbo.LocationPhoto", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.AreaPhoto", "PhotoId", "dbo.Photos");
            DropForeignKey("dbo.AreaPhoto", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.MemberGroup", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.MemberGroup", "MemberId", "dbo.Members");
            DropForeignKey("dbo.LocationSuggestion", "PlanId", "dbo.Plans");
            DropForeignKey("dbo.Plans", "CreatorId", "dbo.Creators");
            DropForeignKey("dbo.Plans", "AreaId", "dbo.Areas");
            DropForeignKey("dbo.Groups", "CreatorId", "dbo.Creators");
            DropForeignKey("dbo.ClaimOwnerRequests", "UserId", "dbo.Users");
            DropForeignKey("dbo.ChangeRequests", "UserId", "dbo.Users");
            DropForeignKey("dbo.BusinessHours", "LocationId", "dbo.Locations");
            DropIndex("dbo.Creators", new[] { "Id" });
            DropIndex("dbo.Members", new[] { "Id" });
            DropIndex("dbo.AnswerTag", new[] { "TagId" });
            DropIndex("dbo.AnswerTag", new[] { "AnswerId" });
            DropIndex("dbo.QuestionArea", new[] { "AreaId" });
            DropIndex("dbo.QuestionArea", new[] { "QuestionId" });
            DropIndex("dbo.LocationTag", new[] { "TagId" });
            DropIndex("dbo.LocationTag", new[] { "LocationId" });
            DropIndex("dbo.MappingLocationSuggestionLocations", new[] { "SuggestionId" });
            DropIndex("dbo.MappingLocationSuggestionLocations", new[] { "LocationId" });
            DropIndex("dbo.UserPlanVote", new[] { "VotePlanId" });
            DropIndex("dbo.UserPlanVote", new[] { "VoterId" });
            DropIndex("dbo.MemberGroup", new[] { "GroupId" });
            DropIndex("dbo.MemberGroup", new[] { "MemberId" });
            DropIndex("dbo.PlanLocations", new[] { "LocationId" });
            DropIndex("dbo.PlanLocations", new[] { "PlanId" });
            DropIndex("dbo.Notes", new[] { "PlanId" });
            DropIndex("dbo.Reports", new[] { "ReviewId" });
            DropIndex("dbo.Reports", new[] { "UserId" });
            DropIndex("dbo.Reviews", new[] { "LocationId" });
            DropIndex("dbo.Reviews", new[] { "CreatorId" });
            DropIndex("dbo.LocationPhoto", new[] { "PhotoId" });
            DropIndex("dbo.LocationPhoto", new[] { "LocationId" });
            DropIndex("dbo.AreaPhoto", new[] { "PhotoId" });
            DropIndex("dbo.AreaPhoto", new[] { "AreaId" });
            DropIndex("dbo.Photos", new[] { "UserId" });
            DropIndex("dbo.Photos", new[] { "ReviewId" });
            DropIndex("dbo.LocationSuggestion", new[] { "PlanId" });
            DropIndex("dbo.LocationSuggestion", new[] { "UserId" });
            DropIndex("dbo.Plans", new[] { "AreaId" });
            DropIndex("dbo.Plans", new[] { "MemberId" });
            DropIndex("dbo.Plans", new[] { "CreatorId" });
            DropIndex("dbo.Plans", new[] { "GroupId" });
            DropIndex("dbo.Groups", new[] { "CreatorId" });
            DropIndex("dbo.GroupInvitations", new[] { "GroupId" });
            DropIndex("dbo.GroupInvitations", new[] { "UserId" });
            DropIndex("dbo.ClaimOwnerRequests", new[] { "UserId" });
            DropIndex("dbo.ClaimOwnerRequests", new[] { "LocationId" });
            DropIndex("dbo.ChangeRequests", new[] { "UserId" });
            DropIndex("dbo.ChangeRequests", new[] { "LocationId" });
            DropIndex("dbo.BusinessHours", new[] { "LocationId" });
            DropIndex("dbo.Locations", new[] { "CreatorId" });
            DropIndex("dbo.Locations", new[] { "AreaId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropTable("dbo.Creators");
            DropTable("dbo.Members");
            DropTable("dbo.AnswerTag");
            DropTable("dbo.QuestionArea");
            DropTable("dbo.LocationTag");
            DropTable("dbo.MappingLocationSuggestionLocations");
            DropTable("dbo.UserPlanVote");
            DropTable("dbo.MemberGroup");
            DropTable("dbo.Tags");
            DropTable("dbo.PlanLocations");
            DropTable("dbo.Notes");
            DropTable("dbo.Reports");
            DropTable("dbo.Reviews");
            DropTable("dbo.LocationPhoto");
            DropTable("dbo.AreaPhoto");
            DropTable("dbo.Photos");
            DropTable("dbo.LocationSuggestion");
            DropTable("dbo.Plans");
            DropTable("dbo.Groups");
            DropTable("dbo.GroupInvitations");
            DropTable("dbo.ClaimOwnerRequests");
            DropTable("dbo.Users");
            DropTable("dbo.ChangeRequests");
            DropTable("dbo.BusinessHours");
            DropTable("dbo.Locations");
            DropTable("dbo.Areas");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}

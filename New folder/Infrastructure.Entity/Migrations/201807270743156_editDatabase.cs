namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Plans", "GroupId", "dbo.Groups");
            DropIndex("dbo.Plans", new[] { "GroupId" });
            DropIndex("dbo.Plans", new[] { "MemberId" });
            AlterColumn("dbo.Plans", "GroupId", c => c.Int());
            AlterColumn("dbo.Plans", "MemberId", c => c.Int());
            CreateIndex("dbo.Plans", "GroupId");
            CreateIndex("dbo.Plans", "MemberId");
            AddForeignKey("dbo.Plans", "GroupId", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Plans", "GroupId", "dbo.Groups");
            DropIndex("dbo.Plans", new[] { "MemberId" });
            DropIndex("dbo.Plans", new[] { "GroupId" });
            AlterColumn("dbo.Plans", "MemberId", c => c.Int(nullable: false));
            AlterColumn("dbo.Plans", "GroupId", c => c.Int(nullable: false));
            CreateIndex("dbo.Plans", "MemberId");
            CreateIndex("dbo.Plans", "GroupId");
            AddForeignKey("dbo.Plans", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
        }
    }
}

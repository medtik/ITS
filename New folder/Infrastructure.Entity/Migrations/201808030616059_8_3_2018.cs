namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8_3_2018 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Plans", "AreaId", c => c.Int(nullable: false));
            CreateIndex("dbo.Plans", "AreaId");
            AddForeignKey("dbo.Plans", "AreaId", "dbo.Areas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Plans", "AreaId", "dbo.Areas");
            DropIndex("dbo.Plans", new[] { "AreaId" });
            DropColumn("dbo.Plans", "AreaId");
        }
    }
}

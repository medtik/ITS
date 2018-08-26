namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asdfg : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Plans", "GroupId", "dbo.Groups");
            AddForeignKey("dbo.Plans", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Plans", "GroupId", "dbo.Groups");
            AddForeignKey("dbo.Plans", "GroupId", "dbo.Groups", "Id");
        }
    }
}

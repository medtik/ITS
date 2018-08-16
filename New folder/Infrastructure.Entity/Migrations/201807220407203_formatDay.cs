namespace Infrastructure.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class formatDay : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BusinessHours", "Day", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BusinessHours", "Day", c => c.Int());
        }
    }
}

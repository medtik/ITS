namespace Infrastructure.Logging.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create_log_object : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        LogId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Location = c.String(),
                        CreatedDate = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.LogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Logs");
        }
    }
}

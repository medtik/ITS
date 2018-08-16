namespace Infrastructure.Logging.Database
{
    using System.Data.Entity;
    using System.Reflection;
    using Core.ObjectModels.Logs;

    public class ITSLoggingContext :DbContext
    {
        public DbSet<Log> Logs { get; set; }

        public ITSLoggingContext() : base("connectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
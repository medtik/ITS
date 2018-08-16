namespace Infrastructure.Identity.Database
{
    using System.Data.Entity;
    using System.Reflection;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Core.ObjectModels.Identity;
    using Infrastructure.Identity.Models;

    public class IdentityITSContext : IdentityDbContext<Account>
    {
        public IdentityITSContext() : base("connectionString")
        {
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
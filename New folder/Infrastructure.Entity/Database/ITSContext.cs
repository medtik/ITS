namespace Infrastructure.Entity.Database
{
    using System.Data.Entity;
    using System.Reflection;
    using Core.ObjectModels.Entities;
    using Infrastructure.Entity.Config;

    public class ITSContext : DbContext
    {
        public ITSContext() : base("connectionString")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<PlanLocation> PlanLocations { get; set; }

        public DbSet<Plan> Plans { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<LocationSuggestion> LocationSuggestions { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<GroupInvitation> GroupInvitations { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<ClaimOwnerRequest> ClaimOwnerRequests { get; set; }

        public DbSet<ChangeRequest> ChangeRequests { get; set; }

        public DbSet<BusinessHour> BusinessHours { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
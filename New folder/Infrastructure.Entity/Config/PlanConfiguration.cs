namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class PlanConfiguration : EntityTypeConfiguration<Plan>
    {
        public PlanConfiguration()
        {
            ToTable("Plans").HasKey(_ => _.Id);

            Property(_ => _.Name).HasMaxLength(255).IsUnicode().IsOptional();
            Property(_ => _.StartDate).IsOptional();
            Property(_ => _.EndDate).IsOptional();
            Property(_ => _.IsPublic).IsOptional();

            HasMany(_ => _.Notes)
                .WithRequired(_ => _.Plan)
                .HasForeignKey(_ => _.PlanId)
                .WillCascadeOnDelete(true);
            HasMany(_ => _.PlanLocations)
                .WithRequired(_ => _.Plan)
                .HasForeignKey(_ => _.PlanId)
                .WillCascadeOnDelete(true);
            HasMany(_ => _.PlanLocations)
                .WithRequired(_ => _.Plan)
                .HasForeignKey(_ => _.PlanId)
                .WillCascadeOnDelete(true);
            HasRequired(_ => _.Area)
                .WithMany(_ => _.Plans)
                .HasForeignKey(_ => _.AreaId)
                .WillCascadeOnDelete(false);
            HasMany(_ => _.LocationSuggestion)
                .WithRequired(_ => _.Plan)
                .HasForeignKey(_ => _.PlanId)
                .WillCascadeOnDelete(true);
        }
        
    }
}
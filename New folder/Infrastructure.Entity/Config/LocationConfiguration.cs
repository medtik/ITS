namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class LocationConfiguration : EntityTypeConfiguration<Location>
    {
        public LocationConfiguration()
        {
            ToTable("Locations").HasKey(_ => _.Id);

            Property(_ => _.Name).HasMaxLength(255).IsUnicode().IsOptional();
            Property(_ => _.Address).HasMaxLength(510).IsUnicode().IsOptional();
            Property(_ => _.Description).HasMaxLength(510).IsUnicode().IsOptional();
            Property(_ => _.Longitude).IsOptional();
            Property(_ => _.Latitude).IsOptional();
            Property(_ => _.Website).HasMaxLength(510).IsUnicode().IsOptional();
            Property(_ => _.PhoneNumber).HasMaxLength(255).IsOptional();
            Property(_ => _.EmailAddress).HasMaxLength(510).IsUnicode().IsOptional();
            Property(_ => _.IsVerified).IsOptional();
            Property(_ => _.IsClosed).IsOptional();
            Property(_ => _.IsDelete).IsOptional();
            Property(_ => _.IsDelete).IsOptional();
            Property(_ => _.LocationRadius).IsOptional();
            Property(_ => _.TotalTimeStay).IsOptional();
            Property(_ => _.TotalStayCount).IsOptional();

            HasMany(_ => _.Tags)
                .WithMany(_ => _.Locations)
                .Map(_ =>
                {
                    _.MapLeftKey("LocationId");
                    _.MapRightKey("TagId");
                    _.ToTable("LocationTag");
                });
            HasMany(_ => _.BusinessHours)
                .WithRequired(_ => _.Location)
                .HasForeignKey(_ => _.LocationId)
                .WillCascadeOnDelete(true);

            HasMany(_ => _.PlanLocations)
                .WithRequired(_ => _.Location)
                .HasForeignKey(_ => _.LocationId)
                .WillCascadeOnDelete(true);

            HasMany(_ => _.ClaimOwnerRequests)
                .WithRequired(_ => _.Location)
                .HasForeignKey(_ => _.LocationId)
                .WillCascadeOnDelete(true);

            HasMany(_ => _.ChangeRequests)
                .WithRequired(_ => _.Location)
                .HasForeignKey(_ => _.LocationId)
                .WillCascadeOnDelete(true);

            HasMany(_ => _.Photos)
                .WithRequired(_ => _.Location)
                .HasForeignKey(_ => _.LocationId)
                .WillCascadeOnDelete(true);

            HasMany(_ => _.LocationSuggestion)
                .WithMany(_ => _.Locations)
                .Map(_ =>
                {
                    _.MapLeftKey("LocationId");
                    _.MapRightKey("SuggestionId");
                    _.ToTable("MappingLocationSuggestionLocations");
                });

            HasMany(_ => _.Reviews)
                .WithRequired(_ => _.Location)
                .HasForeignKey(_ => _.LocationId)
                .WillCascadeOnDelete(false);

        }
    }
}
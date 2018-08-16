namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class LocationSuggestionConfiguration : EntityTypeConfiguration<LocationSuggestion>
    {
        public LocationSuggestionConfiguration()
        {
            ToTable("LocationSuggestion").HasKey(_ => _.Id);

            Property(_ => _.Comment).IsMaxLength().IsUnicode().IsOptional();
            Property(_ => _.Status).IsOptional();
            Property(_ => _.PlanDay).IsOptional();
        }
    }
}
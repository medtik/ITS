namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;
    internal class LocationPhotoConfiguration : EntityTypeConfiguration<LocationPhoto>
    {
        public LocationPhotoConfiguration()
        {
            ToTable("LocationPhoto").HasKey(_ => new { _.LocationId, _.PhotoId });

            Property(_ => _.IsPrimary).IsOptional();
        }
    }
}

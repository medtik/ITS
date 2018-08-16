namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    public class AreaPhotoConfiguration : EntityTypeConfiguration<AreaPhoto>
    {
        public AreaPhotoConfiguration()
        {
            ToTable("AreaPhoto").HasKey(_ => new { _.AreaId, _.PhotoId });

            Property(_ => _.IsPrimary).IsOptional();

            HasRequired(_ => _.Area)
                .WithMany(_ => _.Photos)
                .HasForeignKey(_ => _.AreaId)
                .WillCascadeOnDelete(true);
            HasRequired(_ => _.Photo)
                .WithMany(_ => _.Areas)
                .HasForeignKey(_ => _.PhotoId)
                .WillCascadeOnDelete(true);
        }
    }
}
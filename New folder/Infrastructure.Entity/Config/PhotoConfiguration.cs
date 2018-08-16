namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    public class PhotoConfiguration : EntityTypeConfiguration<Photo>
    {
        public PhotoConfiguration()
        {
            ToTable("Photos").HasKey(_ => _.Id);

            Property(_ => _.Path).IsMaxLength().IsUnicode().IsOptional();

            HasMany(_ => _.Locations)
                .WithRequired(_ => _.Photo)
                .HasForeignKey(_ => _.PhotoId)
                .WillCascadeOnDelete(true);
        }
    }
}

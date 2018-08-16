namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    public class AreaConfiguration : EntityTypeConfiguration<Area>
    {
        public AreaConfiguration()
        {
            ToTable("Areas").HasKey(_ => _.Id);

            Property(_ => _.Name).HasMaxLength(255).IsUnicode().IsOptional();
            Property(_ => _.Description).IsMaxLength().IsUnicode().IsOptional();
            Property(_ => _.Country).HasMaxLength(255).IsUnicode().IsOptional();

            HasMany(_ => _.Locations)
                .WithRequired(_ => _.Area)
                .HasForeignKey(_ => _.AreaId)
                .WillCascadeOnDelete(true);
        }
    }
}
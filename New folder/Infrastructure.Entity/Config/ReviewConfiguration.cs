namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class ReviewConfiguration : EntityTypeConfiguration<Review>
    {
        public ReviewConfiguration()
        {
            ToTable("Reviews").HasKey(_ => _.Id);

            Property(_ => _.Title).HasMaxLength(510).IsUnicode().IsOptional();
            Property(_ => _.Rating).IsOptional();
            Property(_ => _.Description).IsMaxLength().IsUnicode().IsOptional();

            HasMany(_ => _.Photos)
                .WithOptional(_ => _.Review)
                .HasForeignKey(_ => _.ReviewId)
                .WillCascadeOnDelete(true);
            HasMany(_ => _.Reports)
                .WithRequired(_ => _.Review)
                .HasForeignKey(_ => _.ReviewId)
                .WillCascadeOnDelete(true);
        }
    }
}
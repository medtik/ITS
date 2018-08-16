namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class QuestionConfiguration : EntityTypeConfiguration<Question>
    {
        public QuestionConfiguration()
        {
            ToTable("Questions").HasKey(_ => _.Id);

            Property(_ => _.Content).IsMaxLength().IsUnicode().IsOptional();
            Property(_ => _.Categories).HasMaxLength(510).IsUnicode().IsOptional();

            HasMany(_ => _.Areas)
                .WithMany(_ => _.Questions)
                .Map(_ =>
                {
                    _.MapLeftKey("QuestionId");
                    _.MapRightKey("AreaId");
                    _.ToTable("QuestionArea");
                });
        }
    }
}
namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class AnswerConfiguration : EntityTypeConfiguration<Answer>
    {
        public AnswerConfiguration()
        {
            ToTable("Answers").HasKey(_ => _.Id);

            Property(_ => _.Content).IsMaxLength().IsUnicode().IsOptional();

            HasMany(_ => _.Tags)
                .WithMany(_ => _.Answer)
                .Map(_ =>
                {
                    _.MapLeftKey("AnswerId");
                    _.MapRightKey("TagId");
                    _.ToTable("AnswerTag");
                });
            HasRequired(_ => _.Questions)
                .WithMany(_ => _.Answers)
                .HasForeignKey(_ => _.QuestionId)
                .WillCascadeOnDelete(true);
        }
    }
}
namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    public class NoteConfiguration : EntityTypeConfiguration<Note>
    {
        public NoteConfiguration()
        {
            ToTable("Notes").HasKey(_ => _.Id);

            Property(_ => _.Index).IsOptional();
            Property(_ => _.PlanDay).IsOptional();
            Property(_ => _.Title).HasMaxLength(255).IsUnicode().IsOptional();
            Property(_ => _.Done).IsOptional();
            Property(_ => _.Content).IsMaxLength().IsUnicode().IsOptional();
        }
    }
}
namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class ReportConfiguration : EntityTypeConfiguration<Report>
    {
        public ReportConfiguration()
        {
            ToTable("Reports").HasKey(_ => _.Id);

            Property(_ => _.Content).IsMaxLength().IsUnicode().IsOptional();
        }
    }
}
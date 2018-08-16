namespace Infrastructure.Logging.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Logs;

    internal class LogConfiguration : EntityTypeConfiguration<Log>
    {
        public LogConfiguration()
        {
            ToTable("Logs").HasKey(l => l.LogId);

            //property config
            Property(l => l.Message).IsMaxLength().IsUnicode().IsOptional();
            Property(l => l.StackTrace).IsMaxLength().IsUnicode().IsOptional();
            Property(l => l.CreatedDate).IsOptional();
        }
    }
}
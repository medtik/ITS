namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class BusinessHourConfiguration : EntityTypeConfiguration<BusinessHour>
    {
        public BusinessHourConfiguration()
        {
            ToTable("BusinessHours").HasKey(_ => _.Id);

            Property(_ => _.Day).IsOptional();
            Property(_ => _.OpenTime).IsOptional();
            Property(_ => _.CloseTime).IsOptional();
        }
    }
}
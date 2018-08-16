namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class PlanLocationConfiguration : EntityTypeConfiguration<PlanLocation>
    {
        public PlanLocationConfiguration()
        {
            ToTable("PlanLocations").HasKey(_ => _.Id);

            Property(_ => _.PlanDay).IsOptional();
            Property(_ => _.Index).IsOptional();
            Property(_ => _.Comment).IsMaxLength().IsUnicode().IsOptional();
            Property(_ => _.Done).IsOptional();
        }
    }
}
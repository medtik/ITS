namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    public class ChangeRequestConfiguration : EntityTypeConfiguration<ChangeRequest>
    {
        public ChangeRequestConfiguration()
        {
            ToTable("ChangeRequests").HasKey(_ => _.Id);

            Property(_ => _.Name).HasMaxLength(255).IsUnicode().IsOptional();
            Property(_ => _.Address).HasMaxLength(510).IsUnicode().IsOptional();
            Property(_ => _.Description).IsMaxLength().IsUnicode().IsOptional();
            Property(_ => _.Website).IsMaxLength().IsUnicode().IsOptional();
            Property(_ => _.PhoneNumber).HasMaxLength(255).IsUnicode().IsOptional();
            Property(_ => _.EmailAddress).HasMaxLength(510).IsUnicode().IsOptional();
            Property(_ => _.BusinessHours).IsMaxLength().IsUnicode().IsOptional();
            Property(_ => _.Tags).IsMaxLength().IsUnicode().IsOptional();
            Property(_ => _.Status).IsOptional();
        }
    }
}
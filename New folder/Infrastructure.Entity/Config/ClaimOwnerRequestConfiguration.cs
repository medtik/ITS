namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class ClaimOwnerRequestConfiguration : EntityTypeConfiguration<ClaimOwnerRequest>
    {
        public ClaimOwnerRequestConfiguration()
        {
            ToTable("ClaimOwnerRequests").HasKey(_ => _.Id);

            Property(_ => _.Description).IsMaxLength().IsUnicode().IsOptional();
            Property(_ => _.Status).IsOptional();
        }
    }
}

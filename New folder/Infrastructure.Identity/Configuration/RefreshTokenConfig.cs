namespace Infrastructure.Identity.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Identity;

    internal class RefreshTokenConfig : EntityTypeConfiguration<RefreshToken>
    {
        public RefreshTokenConfig()
        {
            ToTable("RefreshToken").HasKey(_ => _.Id);

            Property(_ => _.Id).HasMaxLength(255);
            Property(_ => _.Subject).IsRequired().HasMaxLength(255);
            Property(_ => _.ClientId).IsRequired().HasMaxLength(255); ;
            Property(_ => _.IssuedUtc).IsOptional();
            Property(_ => _.ExpiresUtc).IsOptional();
            Property(_ => _.ProtectedTicket).IsRequired().IsMaxLength();
        }
    }
}
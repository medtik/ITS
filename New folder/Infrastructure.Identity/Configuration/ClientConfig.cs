namespace Infrastructure.Identity.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Identity;

    internal class ClientConfig : EntityTypeConfiguration<Client>
    {
        public ClientConfig()
        {
            ToTable("Clients").HasKey(_ => _.Id);

            Property(_ => _.Id).HasMaxLength(255);
            Property(_ => _.Secret).IsRequired();
            Property(_ => _.Name).IsRequired().HasMaxLength(255);
            Property(_ => _.ApplicationType).IsOptional();
            Property(_ => _.Active).IsOptional();
            Property(_ => _.RefreshTokenLifeTime).IsOptional();
            Property(_ => _.AllowedOrigin).IsOptional().HasMaxLength(255);
        }
    }
}
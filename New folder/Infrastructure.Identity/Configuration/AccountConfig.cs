namespace Infrastructure.Identity.Configuration
{
    using System.Data.Entity.ModelConfiguration;
    using Infrastructure.Identity.Models;

    internal class AccountConfig : EntityTypeConfiguration<Account>
    {
        public AccountConfig()
        {
            ToTable("AspNetUsers");

            Property(_ => _.UserId).IsOptional();
        }
    }
}
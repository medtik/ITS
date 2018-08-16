namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            ToTable("Groups").HasKey(_ => _.Id);

            Property(_ => _.Name).HasMaxLength(255).IsUnicode().IsOptional();

            HasMany(_ => _.Plans)
                .WithOptional(_ => _.Group)
                .HasForeignKey(_ => _.GroupId)
                .WillCascadeOnDelete(false);
            HasMany(_ => _.GroupInvitations)
                .WithRequired(_ => _.Group)
                .HasForeignKey(_ => _.GroupId)
                .WillCascadeOnDelete(true);
        }
    }
}
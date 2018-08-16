namespace Infrastructure.Entity.Config
{
    using System.Data.Entity.ModelConfiguration;
    using Core.ObjectModels.Entities;

    internal class GroupInvitationConfiguration : EntityTypeConfiguration<GroupInvitation>
    {
        public GroupInvitationConfiguration()
        {
            ToTable("GroupInvitations").HasKey(_ => _.Id);

            Property(_ => _.Message).IsMaxLength().IsUnicode().IsOptional();
            Property(_ => _.Status).IsOptional();
        }
    }
}
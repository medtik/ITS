namespace Infrastructure.Entity.Config
{
    using Core.ObjectModels.Entities;
    using System.Data.Entity.ModelConfiguration;

    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(_ => _.Avatar).IsMaxLength().IsUnicode().IsOptional();
            Property(_ => _.Birthdate).IsOptional();
            Property(_ => _.MobileToken).IsMaxLength().IsOptional();
            Property(_ => _.FullName).HasMaxLength(255).IsUnicode().IsOptional();

            HasMany(_ => _.LocationSuggestions)
                .WithRequired(_ => _.User)
                .HasForeignKey(_ => _.UserId)
                .WillCascadeOnDelete(true);
            HasMany(_ => _.GroupInvitations)
                .WithRequired(_ => _.User)
                .HasForeignKey(_ => _.UserId)
                .WillCascadeOnDelete(true);
            HasMany(_ => _.VotePlans)
                .WithMany(_ => _.Voters)
                .Map(_ =>
                {
                    _.MapLeftKey("VoterId");
                    _.MapRightKey("VotePlanId");
                    _.ToTable("UserPlanVote");
                });
            
            
            HasMany(_ => _.Reports)
                .WithRequired(_ => _.User)
                .HasForeignKey(_ => _.UserId)
                .WillCascadeOnDelete(true);
            HasMany(_ => _.Photos)
                .WithRequired(_ => _.User)
                .HasForeignKey(_ => _.UserId)
                .WillCascadeOnDelete(true);
            HasMany(_ => _.ChangeRequests)
                .WithRequired(_ => _.User)
                .HasForeignKey(_ => _.UserId)
                .WillCascadeOnDelete(true);
            HasMany(_ => _.ClaimOwnerRequests)
                .WithRequired(_ => _.User)
                .HasForeignKey(_ => _.UserId)
                .WillCascadeOnDelete(true);
        }
    }

    internal class MemberConfiguration : EntityTypeConfiguration<Member>
    {
        public MemberConfiguration()
        {
            ToTable("Members").HasKey(_ => _.Id);

            HasMany(_ => _.Groups)
                .WithMany(_ => _.Members)
                .Map(_ =>
                {
                    _.MapLeftKey("MemberId");
                    _.MapRightKey("GroupId");
                    _.ToTable("MemberGroup");
                });

            HasMany(_ => _.Plans)
                .WithOptional(_ => _.Member)
                .HasForeignKey(_ => _.MemberId)
                .WillCascadeOnDelete(false);

        }
    }

    internal class CreatorConfiguration : EntityTypeConfiguration<Creator>
    {
        public CreatorConfiguration()
        {
            ToTable("Creators").HasKey(_ => _.Id);

            HasMany(_ => _.Groups)
                .WithRequired(_ => _.Creator)
                .HasForeignKey(_ => _.CreatorId)
                .WillCascadeOnDelete(true);

            HasMany(_ => _.Plans)
                .WithRequired(_ => _.Creator)
                .HasForeignKey(_ => _.CreatorId)
                .WillCascadeOnDelete(true);
            HasMany(_ => _.Locations)
                .WithOptional(_ => _.Creator)
                .HasForeignKey(_ => _.CreatorId)
                .WillCascadeOnDelete(true);
            HasMany(_ => _.Reviews)
                .WithRequired(_ => _.Creator)
                .HasForeignKey(_ => _.CreatorId)
                .WillCascadeOnDelete(true);
        }
    }
}
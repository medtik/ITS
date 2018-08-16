namespace Core.ObjectModels.Entities
{
    using System;
    using System.Collections.Generic;

    public class User : _BaseEntity
    {
        public string FullName { get; set; }

        //Email in account

        //Password in account

        //Phone in account

        public DateTimeOffset Birthdate { get; set; }

        public string Avatar { get; set; } //base 64

        public string Address { get; set; }

        public string MobileToken { get; set; }

        //Is admin in IdentityUserRole

        //Is active in account 

        public ICollection<ClaimOwnerRequest> ClaimOwnerRequests { get; set; }

        public ICollection<ChangeRequest> ChangeRequests { get; set; }

        public ICollection<Report> Reports { get; set; }//report of review

        public ICollection<LocationSuggestion> LocationSuggestions { get; set; }

        public ICollection<GroupInvitation> GroupInvitations { get; set; }

        public ICollection<Plan> VotePlans { get; set; }  // plans that are voted

        public ICollection<Photo> Photos { get; set; }
    }

    public class Member : User
    {
        public ICollection<Group> Groups { get; set; }

        public ICollection<Plan> Plans { get; set; }
    }

    // This is creator, he can create groups/plans and join groups/plans as a member.
    // So, he has 2 kinds of group like below.
    public class Creator : Member
    {
        public ICollection<Group> CreatedGroups { get; set; }

        public ICollection<Plan> CreatedPlans { get; set; }

        public ICollection<Location> Locations { get; set; }

        public ICollection<Review> Reviews { get; set; } //write
    }
}
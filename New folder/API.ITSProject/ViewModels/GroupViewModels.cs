namespace API.ITSProject.ViewModels
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Name = "User")]
    public class UserRemoveViewModels
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int GroupId { get; set; }
    }

    [DataContract(Name = "User")]
    public class UserInvitationViewModels
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int GroupId { get; set; }

        [DataMember]
        public string Message { get; set; }
    }

    [DataContract(Name = "LocationSuggestionOfGroup")]
    public class GroupLocationSuggestionViewModels
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string UserAvatar { get; set; }

        [DataMember]
        public int PlanId { get; set; }

        [DataMember]
        public string PlanName { get; set; }

        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public string LocationName { get; set; }

        [DataMember]
        public string LocationPhoto { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public int Status { get; set; }
    }

    [DataContract(Name = "Group")]
    public class CreatedGroupViewModels
    {
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract(Name = "Groups")]
    public class GroupViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int MemberCount { get; set; }

        [DataMember]
        public int PlanCount { get; set; }

        [DataMember]
        public bool IsOwner { get; set; }
    }

    [DataContract(Name = "Groups")]
    public class GroupDetailViewModels
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public GroupDetailUserViewModels Creator { get; set; }

        [DataMember]
        public IEnumerable<GroupDetailUserViewModels> Users { get; set; }

        [DataMember]
        public IEnumerable<MyPlanViewModels> Plans { get; set; }

        [DataMember]
        public int AreaId { get; set; }

        [DataMember]
        public string AreaName { get; set; }

        [DataMember]
        public bool IsOwner { get; set; }
    }
    
    [DataContract(Name = "User")]
    public class GroupDetailUserViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string Avatar { get; set; }
    }
}
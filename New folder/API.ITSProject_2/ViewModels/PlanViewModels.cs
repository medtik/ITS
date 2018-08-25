using System.ComponentModel.DataAnnotations;

namespace API.ITSProject_2.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Name = "UpdatePlan")]
    public class UpdatePlanViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTimeOffset StartDate { get; set; }

        [DataMember]
        public DateTimeOffset EndDate { get; set; }
    }

    [DataContract(Name = "IndexPlanLocationAndNote")]
    public class UpdateIndexPlanLocationAndNote
    {
        [DataMember]
        public IEnumerable<UpdateIndexPlanLocation> PlanLocation { get; set; }

        [DataMember]
        public IEnumerable<UpdateIndexPlanNote> PlanNotes { get; set; }
    }

    [DataContract(Name = "IndexPlanNote")]
    public class UpdateIndexPlanNote
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public int PlanDay { get; set; }
    }

    [DataContract(Name = "IndexPlanLocation")]
    public class UpdateIndexPlanLocation
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public int PlanDay { get; set; }
    }

    [DataContract(Name = "Suggestion")]
    public class LocationSuggestionViewModels
    {
        [DataMember]
        public int PlanId { get; set; }

        [DataMember]
        public IEnumerable<int> LocationIds { get; set; }

        [DataMember]
        public int PlanDay { get; set; }

        [DataMember]
        public string Comment { get; set; }
    }

    [DataContract(Name = "Note")]
    public class CreatePlanNoteViewModles
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public int? PlanDay { get; set; }

        [DataMember]
        public int? Index { get; set; }

        [DataMember]
        public int PlanId { get; set; }
    }

    [DataContract(Name = "PlanLocation")]
    public class UpdatePlanLocationViewModels
    {
        [DataMember]
        public int PlanLocationId { get; set; }

        [DataMember]
        public int Index { get; set; }
    }

    [DataContract(Name = "PlanNote")]
    public class UpdatePlanNoteViewModels
    {
        [DataMember]
        public int NoteId { get; set; }

        [DataMember]
        public int Index { get; set; }
    }

    [DataContract(Name = "Plan")]
    public class CreatePlanViewModels
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTimeOffset StartDate { get; set; }

        [DataMember]
        public DateTimeOffset EndDate { get; set; }

        [DataMember]
        public int AreaId { get; set; }
    }
    
    [DataContract(Name = "Plan")]
    public class CreateSuggestedPlanViewModels
    {
        [DataMember, Required]
        public int[] Answers { get; set; }
        
        [DataMember, Required]
        public string Name { get; set; }

        [DataMember, Required]
        public DateTimeOffset StartDate { get; set; }

        [DataMember, Required]
        public DateTimeOffset EndDate { get; set; }

        [DataMember,Required]
        public int AreaId { get; set; }
    }

    [DataContract(Name = "LocationInPlan")]
    public class LocationInPlanViewModels
    {
        [DataMember]
        public int? PlanDay { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public int PlanId { get; set; }

        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public int? Index { get; set; }
    }

    [DataContract(Name = "Plan")]
    public class FeaturedPlanViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Photo { get; set; }

        [DataMember]
        public int Voter { get; set; }

        [DataMember]
        public int Time { get; set; }

        [DataMember]
        public int AreaId { get; set; }

        [DataMember]
        public string AreaName { get; set; }

        [DataMember]
        public int CreatorId { get; set; }

        [DataMember]
        public string CreatorName { get; set; }
    }

    [DataContract(Name = "Plan")]
    public class MyPlanViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTimeOffset StartDate { get; set; }

        [DataMember]
        public DateTimeOffset EndDate { get; set; }

        [DataMember]
        public IEnumerable<PlanLocationViewModels> Locations { get; set; }

        [DataMember]
        public int AreaId { get; set; }

        [DataMember]
        public string AreaName { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public bool IsGroupOwner { get; set; }

        [DataMember]
        public bool IsPlanOwner { get; set; }

        [DataMember]
        public bool IsPublic { get; set; }
    }

    [DataContract(Name = "Location")]
    public class PlanLocationViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Photo { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public double Rating { get; set; }

        [DataMember]
        public string Category { get; set; }
    }

    [DataContract(Name = "Note")]
    public class PlanDetailNoteViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public int PlanDay { get; set; }

        [DataMember]
        public bool IsDone { get; set; }
    }

    [DataContract(Name = "Location")]
    public class PlanDetailLocationViewModels
    {
        [DataMember]
        public int PlanLocationId { get; set; }

        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public float Rating { get; set; }

        [DataMember]
        public int ReviewCount { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Photo { get; set; }

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public int PlanDay { get; set; }
        
        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public bool IsDone { get; set; }

        [DataMember]
        public int? TotalTimeStay { get; set; }
    }

    [DataContract(Name = "Plan")]
    public class PlanDetailViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTimeOffset StartDate { get; set; }

        [DataMember]
        public DateTimeOffset EndDate { get; set; }

        [DataMember]
        public IEnumerable<PlanDetailNoteViewModels> Notes { get; set; }

        [DataMember]
        public IEnumerable<PlanDetailLocationViewModels> Locations { get; set; }

        [DataMember]
        public int AreaId { get; set; }

        [DataMember]
        public string AreaName { get; set; }

        [DataMember]
        public bool IsOwner { get; set; }

        [DataMember]
        public bool IsDone { get; set; }

        [IgnoreDataMember]
        public int MemberId { get; set; }

        [DataMember]
        public bool IsPublic { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public bool IsVoted { get; set; }
    }
}
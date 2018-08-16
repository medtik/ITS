namespace Core.ObjectModels.Entities
{
    using System;
    using System.Collections.Generic;

    public class Plan : _BaseEntity
    {
        public string Name { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public bool IsPublic { get; set; }

        public ICollection<LocationSuggestion> LocationSuggestion { get; set; }

        public int? GroupId { get; set; }

        public Group Group { get; set; }

        public int CreatorId { get; set; }

        public Creator Creator { get; set; } //created by

        public int? MemberId { get; set; } 

        public Member Member { get; set; } //clone

        public ICollection<User> Voters { get; set; } // users who vote

        public ICollection<Note> Notes { get; set; }

        public ICollection<PlanLocation> PlanLocations { get; set; }

        public int AreaId { get; set; }

        public Area Area { get; set; }
    }
}
namespace Core.ObjectModels.Entities
{
    using System.Collections.Generic;

    public class Location : _BaseEntity
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string Website { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public bool IsVerified { get; set; }

        public bool IsClosed { get; set; }

        public bool IsDelete { get; set; }

        public string Category { get; set; }

        // Start relationships
        public int AreaId { get; set; }

        public int? TotalTimeStay { get; set; }

        public int? TotalStayCount { get; set; }

        public double? LocationRadius { get; set; }

        public int? CreatorId { get; set; }

        public Area Area { get; set; }

        public Creator Creator { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<BusinessHour> BusinessHours { get; set; }

        public ICollection<LocationPhoto> Photos { get; set; }

        public ICollection<PlanLocation> PlanLocations { get; set; }

        public ICollection<ChangeRequest> ChangeRequests { get; set; }

        public ICollection<ClaimOwnerRequest> ClaimOwnerRequests { get; set; }

        public ICollection<Review> Reviews { get; set; }
            
        public ICollection<LocationSuggestion> LocationSuggestion { get; set; }
    }
}
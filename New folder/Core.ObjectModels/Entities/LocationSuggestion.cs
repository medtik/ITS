using Core.ObjectModels.Entities.EnumType;
using System.Collections.Generic;

namespace Core.ObjectModels.Entities
{
    public class LocationSuggestion : _BaseEntity
    {
        public RequestStatus Status { get; set; }

        public string Comment { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int? PlanDay { get; set; }

        public int PlanId { get; set; }

        public Plan Plan { get; set; }////associate

        public ICollection<Location> Locations { get; set; }
    }
}
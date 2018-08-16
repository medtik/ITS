namespace Core.ObjectModels.Entities
{
    using System.Collections.Generic;

    public class Area : _BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public ICollection<Location> Locations { get; set; }

        public ICollection<AreaPhoto> Photos { get; set; }

        public ICollection<Question> Questions { get; set; }

        public ICollection<Plan> Plans { get; set; }
    }
}
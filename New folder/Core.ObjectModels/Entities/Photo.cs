namespace Core.ObjectModels.Entities
{
    using System.Collections.Generic;

    public class Photo : _BaseEntity
    {
        public string Path { get; set; }

        public int? ReviewId { get; set; }

        public int? UserId { get; set; }

        public ICollection<AreaPhoto> Areas { get; set; }

        public ICollection<LocationPhoto> Locations { get; set; }

        public User User { get; set; }

        public Review Review { get; set; }
    }
}
using System.Collections.Generic;

namespace Core.ObjectModels.Entities
{
    public class Review : _BaseEntity
    {
        public string Title { get; set; }

        public float Rating { get; set; }

        public string Description { get; set; }

        public ICollection<Report> Reports { get; set; }

        public int CreatorId { get; set; }

        public Creator Creator { get; set; }

        public int LocationId { get; set; }

        public Location Location { get; set; }

        public ICollection<Photo> Photos { get; set; }
    }
}
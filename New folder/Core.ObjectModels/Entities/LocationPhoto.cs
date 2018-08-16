namespace Core.ObjectModels.Entities
{
    public class LocationPhoto
    {
        public int LocationId { get; set; }

        public int PhotoId { get; set; }

        public Photo Photo { get; set; }

        public Location Location { get; set; }

        public bool IsPrimary { get; set; }
    }
}

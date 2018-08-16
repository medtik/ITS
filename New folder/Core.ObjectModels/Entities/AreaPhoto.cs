namespace Core.ObjectModels.Entities
{
    public class AreaPhoto
    {
        public int AreaId { get; set; }

        public int PhotoId { get; set; }

        public Area Area { get; set; }

        public Photo Photo { get; set; }

        public bool IsPrimary { get; set; }
    }
}
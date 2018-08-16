namespace Core.ObjectModels.Entities
{
    using System;

    public class BusinessHour : _BaseEntity
    {
        public string Day { get; set; }

        public TimeSpan OpenTime { get; set; }

        public TimeSpan CloseTime { get; set; }

        public int LocationId { get; set; }

        public Location Location { get; set; }
    }
}
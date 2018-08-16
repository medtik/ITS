namespace Core.ObjectModels.Entities
{
    public class ChangeRequest : _BaseEntity
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string Website { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string BusinessHours { get; set; }

        public string Tags { get; set; }

        public int Status { get; set; }

        public int LocationId { get; set; }

        public int UserId { get; set; }

        public Location Location { get; set; }

        public User User { get; set; }
    }
}
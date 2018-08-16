namespace Core.ObjectModels.Entities
{
    public class ClaimOwnerRequest : _BaseEntity
    {
        public string Description { get; set; }

        public int Status { get; set; }

        public int LocationId { get; set; }

        public int UserId { get; set; }

        public Location Location { get; set; }

        public User User { get; set; }
    }
}
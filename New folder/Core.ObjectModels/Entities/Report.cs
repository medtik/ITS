namespace Core.ObjectModels.Entities
{
    public class Report : _BaseEntity
    {
        public string Content { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int ReviewId { get; set; }

        public Review Review { get; set; }
    }
}
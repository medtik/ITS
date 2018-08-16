namespace Core.ObjectModels.Entities
{
    using Core.ObjectModels.Entities.EnumType;

    public class GroupInvitation : _BaseEntity
    {
        public string Message { get; set; }

        public RequestStatus Status { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
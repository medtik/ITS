namespace Core.ObjectModels.Entities
{
    using System.Collections.Generic;

    public class Group : _BaseEntity
    {
        public string Name { get; set; }

        public ICollection<GroupInvitation> GroupInvitations { get; set; }

        public ICollection<Member> Members { get; set; }

        public int CreatorId { get; set; }

        public Creator Creator { get; set; }

        public ICollection<Plan> Plans { get; set; }
    }
}
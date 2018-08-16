namespace Core.ObjectModels.Entities
{
    public class Note : _BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public int PlanDay { get; set; }

        public int Index { get; set; }

        public bool Done { get; set; }

        public int PlanId { get; set; }

        public Plan Plan { get; set; }
    }
}
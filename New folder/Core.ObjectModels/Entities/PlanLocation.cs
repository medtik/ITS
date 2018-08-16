namespace Core.ObjectModels.Entities
{
    public class PlanLocation : _BaseEntity
    {
        public int PlanDay { get; set; }

        public int Index { get; set; }

        public string Comment { get; set; }

        public bool Done { get; set; }

        public int PlanId { get; set; }

        public int LocationId { get; set; }

        public Location Location { get; set; }

        public Plan Plan { get; set; }
    }
}
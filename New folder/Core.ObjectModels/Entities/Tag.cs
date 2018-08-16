namespace Core.ObjectModels.Entities
{
    using System.Collections.Generic;
    
    public class Tag : _BaseEntity
    {
        public string Name { get; set; }

        public string Categories { get; set; }//static value

        public ICollection<Answer> Answer { get; set; }

        public ICollection<Location> Locations { get; set; }
    }
}
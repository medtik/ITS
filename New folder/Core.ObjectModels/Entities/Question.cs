using System.Collections.Generic;

namespace Core.ObjectModels.Entities
{
    public class Question : _BaseEntity
    {
        public string Content { get; set; }

        public string Categories { get; set; }//static value

        public ICollection<Answer> Answers { get; set; }

        public ICollection<Area> Areas { get; set; }
    }
}
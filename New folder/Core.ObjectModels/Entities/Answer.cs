namespace Core.ObjectModels.Entities
{
    using System.Collections.Generic;

    public class Answer : _BaseEntity
    {
        public string Content { get; set; }

        public int QuestionId { get; set; }

        public Question Questions { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}
namespace API.ITSProject.ViewModels
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Name = "Question")]
    public class QuestionViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string Categories { get; set; }

        [DataMember]
        public int AnswerCount { get; set; }
    }

    [DataContract(Name = "Question")]
    public class QuestionDetailsViewModels
    {
        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public IEnumerable<AnswerViewModels> Answer { get; set; }
    }

    [DataContract(Name = "Answer")]
    public class AnswerViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Content { get; set; }
    }

    [DataContract(Name = "Question")]
    public class CreateQuestionViewModels
    {
        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string Categories { get; set; }

        [DataMember]
        public IEnumerable<AnswerForQuestionViewModels> Answers { get; set; }
    }

    public class AnswerForQuestionViewModels
    {

        [DataMember]
        public string Answer { get; set; }

        [DataMember]
        public IEnumerable<int> Tags { get; set; }
    }
}
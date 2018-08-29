namespace API.ITSProject_2.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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

        [DataMember]
        public string Category { get; set; }

        public int Id { get; set; }
    }

    [DataContract(Name = "Answer")]
    public class AnswerViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public IEnumerable<TagViewModels> Tags { get; set; }
    }

    [DataContract(Name = "Question")]
    public class CreateQuestionViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Content { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Thể loại không được để trống")]
        public string Categories { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Câu trả lời không được trống")]
        public IEnumerable<AnswerForQuestionViewModels> Answers { get; set; }
    }

    public class AnswerForQuestionViewModels
    {

        [DataMember]
        [Required(ErrorMessage = "Câu trả lời không được trống")]
        [MinLength(1, ErrorMessage = "Câu trả lời không được trống")]
        public string Answer { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Câu trả lời phải được gắn thẻ")]
        public IEnumerable<int> Tags { get; set; }
    }
}
namespace API.ITSProject_2.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract(Name = "Tag")]
    public class TagViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Categories { get; set; }

        [DataMember]
        public int LocationCount { get; set; }
    }

    [DataContract(Name = "Tag")]
    public class CreateTagViewModels
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Thể loại không được để trống")]
        public string Categories { get; set; }
    }
}
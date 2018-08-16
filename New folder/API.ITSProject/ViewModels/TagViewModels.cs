namespace API.ITSProject.ViewModels
{
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
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Categories { get; set; }
    }
}
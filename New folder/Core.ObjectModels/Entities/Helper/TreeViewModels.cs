namespace Core.ObjectModels.Entities.Helper
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class TreeViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string Percent { get; set; }

        [DataMember]
        public float Rating { get; set; }

        [DataMember]
        public int ReviewCount { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string PrimaryPhoto { get; set; }

        [DataMember]
        public IList<string> Reasons { get; set; }

        [DataMember]
        public string Categories { get; set; }
    }
}
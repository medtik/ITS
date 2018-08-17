namespace API.ITSProject.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class ReviewViewModels
    {
        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember, Range(0.0, 5.0, ErrorMessage = "Invalid range (0.0 - 5.0)")]
        public float Rating { get; set; }

        [DataMember]
        public IEnumerable<string> Photos { get; set; }
    }

    [DataContract(Name = "Location")]
    public class LocationViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Website { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        public string AreaName { get; set; }

        [DataMember]
        public string PrimaryPhoto { get; set; }

        [DataMember]
        public float Rating { get; set; }

        [DataMember]
        public int ReviewCount { get; set; }

        [DataMember]
        public string Categories { get; set; }
    }

    [DataContract(Name = "Location")]
    public class CreateLocationViewModels
    {
        [DataMember]
        public double Radius { get; set; }

        [DataMember, Required]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public string Website { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        public int AreaId { get; set; }

        [DataMember]
        public int[] Tags { get; set; }//list tag id

        [DataMember]
        public string PrimaryPhoto { get; set; }//base64

        [DataMember]
        public string[] OtherPhotos { get; set; }//base64

        [DataMember]
        public ICollection<BusinessHourViewModels> Days { get; set; }

        [DataMember]
        public string Category { get; set; }
    }

    [DataContract(Name = "Location")]
    public class LocationDetailViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public float Rating { get; set; }

        [DataMember]
        public int RatingCount { get; set; }

        [DataMember]
        public IEnumerable<string> Tags { get; set; }

        [DataMember]
        public IEnumerable<BusinessHourViewModels> BusinessHours { get; set; }

        [DataMember]
        public string PrimaryPhoto { get; set; }

        [DataMember]
        public IEnumerable<string> OtherPhotos { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string Website { get; set; }

        /// <summary>
        /// Top 5 current
        /// </summary>
        [DataMember]
        public IEnumerable<CommentViewModels> Comments { get; set; }

        [DataMember]
        public string Category { get; set; }
    }
}
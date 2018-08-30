namespace API.ITSProject_2.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    
    [DataContract]
    public class PhotoForLocationViewModels
    {
        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public string Avatar { get; set; }
    }

    [DataContract]
    public class EditLocationViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Tên không được trống")]
        public string Name { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Địa chỉ không được trống")]
        public string Address { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Vĩ độ không được trống")]
        public double? Latitude { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Kinh độ không được trống")]
        public double? Longitude { get; set; }

        [DataMember]
        public string Website { get; set; }

        [DataMember]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        [DataMember]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Khu vực không được trống")]
        public int? AreaId { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Thể loại không được trống")]
        public string Category { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        public int[] Tags { get; set; }//list tag id

        [DataMember]
        public string PrimaryPhoto { get; set; }//base64

        [DataMember]
        public string[] OtherPhotos { get; set; }//base64

        [DataMember]
        public ICollection<BusinessHourViewModels> Days { get; set; }

        [DataMember]
        public ICollection<int> ReviewIds { get; set; }
    }

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

        [DataMember]
        public double Long { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public string Range { get; set; }
    }

    [DataContract(Name = "Location")]
    public class CreateLocationViewModels
    {
        [DataMember] 
        public double Radius { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Tên không được trống")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được trống")]
        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Kinh độ không được trống")]
        [Range(-180, 180, ErrorMessage = "Kinh độ không hợp lệ(-180, 180)")]
        public double? Longitude { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Vĩ độ không được trống")]
        [Range(-90, 90, ErrorMessage = "Vĩ độ không hợp lệ(-90, 90)")]
        public double? Latitude { get; set; }

        [DataMember]
        public string Website { get; set; }

        [DataMember]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [DataMember]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string EmailAddress { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }

        [DataMember]
        public bool IsClosed { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Khu vực không được trống")]
        public int? AreaId { get; set; }

        [DataMember]
        public int[] Tags { get; set; }//list tag id

        [DataMember]
        public string PrimaryPhoto { get; set; }//base64

        [DataMember]
        public string[] OtherPhotos { get; set; }//base64

        [DataMember]
        [Required(ErrorMessage = "Vui lòng điền ngày/giờ kinh doanh")]
        public ICollection<BusinessHourViewModels> Days { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Thể loại không được trống")]
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
        public IEnumerable<TagViewModels> Tags { get; set; }

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
        public string Description { get; set; }

        [DataMember]
        public double Long { get; set; }

        [DataMember]
        public double Lat { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string Area { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }

        [DataMember]
        public bool IsClose { get; set; }
    }
}
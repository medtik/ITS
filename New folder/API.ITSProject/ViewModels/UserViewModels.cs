namespace API.ITSProject.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract(Name = "User")]
    public class SearchUserViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Avatar { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public DateTimeOffset Birthdate { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public bool IsBanned { get; set; }
    }

    [DataContract(Name = "User")]
    public class ShowUserViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Avatar { get; set; }

        [DataMember]
        public string FullName { get; set; }
    }

    [DataContract(Name = "User")]
    public class UserViewModels
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public DateTimeOffset Birthdate { get; set; }

        [DataMember]
        public string Avatar { get; set; }
    }

    [DataContract]
    public class RegisterViewModels
    {
        [DataMember, EmailAddress(ErrorMessage = "Email của bạn không hợp lệ"), Required(ErrorMessage = "Bắt buộc nhập")]
        public string EmailAddress { get; set; }

        [DataMember, Phone(ErrorMessage = "Vui lòng kiểm tra định dạng điện thoại của bạn")]
        public string PhoneNumber { get; set; }

        [DataMember, Required(ErrorMessage = "Bắt buộc nhập")]
        public string Password { get; set; }

        [DataMember, Compare(nameof(Password), ErrorMessage = "Mật khẩu bạn nhập không khớp")]
        public string RePassword { get; set; }

        [DataMember, Required(ErrorMessage = "Bắt buộc nhập")]
        public string FullName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public DateTimeOffset Birthdate { get; set; }
    }

    [DataContract(Name = "User")]
    public class UserRecoverPassword
    {
        [DataMember]
        public string Email { get; set; }
    }
}
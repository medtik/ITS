namespace API.ITSProject_2.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class BusinessHourViewModels
    {
        [Required(ErrorMessage = "Vui lòng chọn ngày")]
        public string Day { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giờ mở cửa")]
        public TimeSpan From { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giờ đóng cửa")]
        public TimeSpan To { get; set; }
    }
}
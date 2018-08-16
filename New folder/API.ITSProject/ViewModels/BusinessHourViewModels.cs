namespace API.ITSProject.ViewModels
{
    using System;

    public class BusinessHourViewModels
    {
        public string Day { get; set; }

        public TimeSpan From { get; set; }

        public TimeSpan To { get; set; }
    }
}
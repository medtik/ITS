namespace Core.ObjectModels.Logs
{
    using System;

    public class Log
    {
        public int LogId { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public string Location { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
namespace Core.ApplicationService.Business.LogService
{
    using System;

    public interface ILoggingService
    {
        void Write(string className, string action, Exception ex);
    }
}
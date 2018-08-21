using System.Collections.Generic;

namespace Core.ApplicationService.Business.LogService
{
    using System;

    public interface ILoggingService
    {
        void Write(string className, string action, Exception ex);
        void CaptureSentryException(Exception ex);
        void AddSentryBreadCrum(String title, Dictionary<string, object> data = null , String message = null);
    }
}
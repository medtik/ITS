namespace Infrastructure.Logging.Database
{
    using Core.ApplicationService.Database.Log;

    public class LogContext : ILogContext
    {
        public object GetContext => new ITSLoggingContext();
    }
}
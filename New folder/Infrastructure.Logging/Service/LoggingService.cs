namespace Infrastructure.Logging.Service
{
    using System;
    using System.Data.Entity;
    using Core.ObjectModels.Logs;
    using Core.ApplicationService.Database.Log;
    using Core.ApplicationService.Business.LogService;

    public class LoggingService : ILoggingService
    {
        private DbContext _dbContext;

        public LoggingService(ILogContext context)
        {
            _dbContext = context.GetContext as DbContext;
        }

        public void Write(string className, string action, Exception ex)
        {
            try
            {
                _dbContext.Set<Log>().Add(new Log()
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Location = $"{className}/{action}",
                    CreatedDate = DateTimeOffset.Now
                });

                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
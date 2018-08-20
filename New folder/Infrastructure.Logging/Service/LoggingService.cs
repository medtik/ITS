using System.Collections.Generic;
using SharpRaven;
using SharpRaven.Data;

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
        private readonly RavenClient _ravenClient;

        public LoggingService(ILogContext context)
        {
            _dbContext = context.GetContext as DbContext;
            
            _ravenClient = new RavenClient("https://b577de4f9b1c408d839aa1600461383c@sentry.io/1265094");
        }

        public void Write(string className, string action, Exception ex)
        {
            try
            {
                CaptureSentryException(ex);
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

        public void CaptureSentryException(Exception ex)
        {
            _ravenClient.Capture(new SentryEvent(ex));
        }

        public void AddSentryBreadCrum(String title,String message,  Dictionary<string,string> data)
        {
            _ravenClient.AddTrail(new Breadcrumb(title){
                Level = BreadcrumbLevel.Info,
                Data = data,
                Message = message
            });
        }
    }
}
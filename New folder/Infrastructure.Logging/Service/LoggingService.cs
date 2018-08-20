using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public void AddSentryBreadCrum(String title, Dictionary<string, object> data = null, String message = null)
        {
            var breadcrumb = new Breadcrumb(title)
            {
                Level = BreadcrumbLevel.Info,
            };

            if (data != null)
            {
                var dataDic = new Dictionary<string, string>();
                foreach (KeyValuePair<string, object> pair in data)
                {
                    string key = pair.Key;
                    string value;

                    try
                    {
                        value = JsonConvert.SerializeObject(pair.Value);
                    }
                    catch (Exception)
                    {
                        value = "Can't convert to json";
                    }

                    dataDic.Add(key, value);
                }

                breadcrumb.Data = dataDic;
            }

            if (message != null)
            {
                breadcrumb.Message = message;
            }

            _ravenClient.AddTrail(breadcrumb);
        }
    }
}
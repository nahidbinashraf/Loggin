using log4net.Appender;
using log4net.Core;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Loggin.Custom
{

    public class CustomLogAppneder : AppenderSkeleton
    {
        private string _system = ConfigurationManager.AppSettings["log:system"];
        private string _customerToken = string.Empty;
        private string _rootUrl = string.Empty;
        private string _tag = string.Empty;

        public string CustomerToken
        {
            get { return _customerToken; }
            set { _customerToken = value; }
        }

        public string RootUrl
        {
            get { return _rootUrl; }
            set { _rootUrl = value; }
        }

        public string Tag
        {
            get { return _tag; }
            set { _tag = value + $",{_system}"; }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (string.IsNullOrEmpty(_rootUrl) || string.IsNullOrEmpty(_customerToken) || string.IsNullOrEmpty(_tag))
            {
                Trace.WriteLine($"Invalid key");
                Console.WriteLine($"Invalid key");
            }
            else
                SendLogglyRequestAsync(loggingEvent);
        }

        private async Task SendLogglyRequestAsync(LoggingEvent loggingEvent)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var url = $"{_rootUrl}/inputs/{_customerToken}/tag/{_tag}/";

                    var logData = new
                    {
                        LogglyId = Guid.NewGuid(),
                        LogLevel = loggingEvent.Level?.DisplayName,
                        LogName = loggingEvent.LoggerName,
                        IdentityUser = loggingEvent?.Identity,
                        LogMethod = loggingEvent.LocationInformation?.MethodName,
                        LogLine = loggingEvent.LocationInformation?.LineNumber,
                        ShortMessage = loggingEvent.ExceptionObject?.Message?.ToString() ?? loggingEvent.MessageObject.ToString(),
                        FullMessage = loggingEvent.ExceptionObject?.ToString() ?? loggingEvent.MessageObject.ToString(),
                        CreatedOn = loggingEvent.TimeStamp.ToString(),
                        CreatedOnUtc = loggingEvent.TimeStampUtc.ToString(),
                        System = "System Name"
                    };

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(logData);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content).ConfigureAwait(false);
                    if (!response.IsSuccessStatusCode)
                    {
                        Trace.WriteLine($"Loggly request failed. Status code: {response.StatusCode}");
                        Console.WriteLine($"Loggly request failed. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Exception occurred while sending log to Loggly: {ex.Message}");
                    Console.WriteLine($"Exception occurred while sending log to Loggly: {ex.Message}");
                }
            }
        }
    }
}

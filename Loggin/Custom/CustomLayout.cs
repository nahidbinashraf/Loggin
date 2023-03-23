using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loggin.Custom
{

    public class CustomLayout : LayoutSkeleton
    {
        public override void ActivateOptions()
        {
            // No options to activate
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var logglyData = new
            {
                pass = "OK",
                timestamp = loggingEvent.TimeStamp.ToString("o"),
                message = loggingEvent.RenderedMessage,
                logger = loggingEvent.LoggerName,
                level = loggingEvent.Level.Name,
                thread = loggingEvent.ThreadName,
                exception = loggingEvent.ExceptionObject?.ToString(),
                type = "CustomLayout",
                properties = loggingEvent.Properties


            };
            var json = JsonConvert.SerializeObject(logglyData);
            writer.Write(json);
        }

    }

}

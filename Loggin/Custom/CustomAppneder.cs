using log4net.Core;
using log4net.loggly;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loggin.Custom
{
    public class CustomAppneder : LogglyAppender
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
           
            loggingEvent.Properties["system"] = "Test";
            loggingEvent.Properties["server"] = Environment.MachineName;
            loggingEvent.Properties["CustomJson"] = "Custom Json";


            base.Append(loggingEvent);
        }
    }
}

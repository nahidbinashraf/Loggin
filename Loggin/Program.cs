using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Loggin
{
    class Program
    {
        
        public Program()
        {
           
            
        }
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("log4netconfig.config"));
            var logger = LogManager.GetLogger(typeof(Program));
            try
            {
             
                logger.Info("Invoke loggly from Episerver Test Application One");
                int x = 1;
                int y = 0;
                int z = x / y;
            }
            catch (Exception ex)
            {
                logger.Error("Invoke error loggly from Episerver Test Application Two", ex);
            }
            Console.ReadKey();
        }
    }
}

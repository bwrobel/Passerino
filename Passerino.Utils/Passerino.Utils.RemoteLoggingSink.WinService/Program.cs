using System;
using System.ServiceProcess;
using Passerino.Utils.Logging;

namespace Passerino.Utils.RemoteLoggingSink.WinService
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            var logProcessor = new Log4NetProcessor();
            var servicesToRun = new ServiceBase[] 
                { 
                    new Log4NetRemoteLoggingSinkWinService(new Log4NetRemoteLoggingSinkConfiguration(logProcessor),logProcessor) 
                };

            try
            {
                ServiceBase.Run(servicesToRun);
            }
            catch (Exception ex)
            {
                log.Fatal("Couldn't start service.", ex);
                throw;
            }
            ServiceBase.Run(servicesToRun);
        }
    }
}

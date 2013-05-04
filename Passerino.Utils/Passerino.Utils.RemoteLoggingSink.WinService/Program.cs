using System;
using System.ServiceProcess;
using Passerino.Utils.Logging;
using StructureMap;

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

            var servicesToRun = new ServiceBase[] 
                { 
                    ObjectFactory.GetInstance<Log4NetRemoteLoggingSinkWinService>() 
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

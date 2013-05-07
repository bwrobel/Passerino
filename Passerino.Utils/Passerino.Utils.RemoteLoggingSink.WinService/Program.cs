using System;
using System.ServiceProcess;
using Passerino.Utils.IoC;
using Passerino.Utils.Logging;
using StructureMap;

namespace Passerino.Utils.RemoteLoggingSink.WinService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var logFactory = new Log4NetLogFactory();
            var log = logFactory.New(typeof (Program));

            try
            {
                new StructureMapConfigManager(logFactory).Initialize();
            }
            catch (Exception ex)
            {
                log.Fatal("IoC Unhandled Exception").WithException(ex).Proceed();
                throw;
            }

            var serviceToRun = ObjectFactory.GetInstance<Log4NetRemoteLoggingSinkWinService>();

            try
            {
                ServiceBase.Run(serviceToRun);
                log.Info("Service started").Proceed();
            }
            catch (Exception ex)
            {
                log.Fatal("Couldn't start service.").WithException(ex).Proceed();
                throw;
            }
        }
    }
}

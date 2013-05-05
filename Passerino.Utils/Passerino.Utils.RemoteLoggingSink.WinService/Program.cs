using System;
using System.ServiceProcess;
using Passerino.Utils.Configuration.Log4NetConfig;
using Passerino.Utils.Configuration.StructureMapConfig;
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
            var log = Log4NetConfigManager.GetInstance(Environment.CurrentDirectory).SetSource(typeof(Program));

            try
            {
                new StructureMapConfigManager(Log4NetConfigManager.GetInstance(Environment.CurrentDirectory))
                    .Initialize();
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

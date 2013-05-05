using System;
using System.ServiceProcess;
using Passerino.Utils.Configuration.Management.Log4NetConfig;
using Passerino.Utils.Configuration.Management.StructureMapConfig;
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
            var log = Log4NetConfigManager
                .GetInstance(Environment.CurrentDirectory)
                .SetSource(typeof(Program));

            try
            {
                StructureMapConfigManager.Initialize();
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
            }
            catch (Exception ex)
            {
                log.Fatal("Couldn't start service.").WithException(ex).Proceed();
                throw;
            }
        }
    }
}

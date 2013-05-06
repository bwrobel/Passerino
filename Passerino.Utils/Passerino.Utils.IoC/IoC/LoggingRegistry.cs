using System;
using Passerino.Utils.Logging;
using Passerino.Utils.Logging.Log4Net;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.IoC.IoC
{
    public class LoggingRegistry : Registry
    {
        public LoggingRegistry()
        {
            For<ILogProcessor>().Use(() => Log4NetConfigManager.GetInstance(Environment.CurrentDirectory));
        }

        public static void PostInitialize()
        {
            Log4NetConfigManager.Initialize();
        }
    }
}

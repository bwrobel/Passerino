using Passerino.Utils.Logging;
using Passerino.Utils.Logging.Log4Net;

namespace Passerino.Utils.Configuration.Management.Log4NetConfig
{
    public static class Log4NetConfigManager
    {
        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized) return;
            
            log4net.Config.XmlConfigurator.Configure();

            _initialized = true;
        }

        public static ILogProcessor GetInstance(string applicationName)
        {
            Initialize();
            return new Log4NetProcessor { ApplicationName = applicationName };
        }
    }
}

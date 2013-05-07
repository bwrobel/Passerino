using System;
using Passerino.Utils.Domain.Logging;

namespace Passerino.Utils.Logging
{
    public class Log4NetLogFactory : ILogFactory
    {
        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized) return;
            
            log4net.Config.XmlConfigurator.Configure();

            _initialized = true;
        }

        public ILog New(Type source)
        {
            return new Log4NetLog(Environment.CurrentDirectory)
                .SetSource(source);   
        }
    }
}

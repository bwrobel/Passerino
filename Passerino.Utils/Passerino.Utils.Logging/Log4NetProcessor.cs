using System;
using System.Collections.Generic;
using log4net;

namespace Passerino.Utils.Logging
{
    public class Log4NetProcessor : ILogProcessor
    {
        internal static string ApplicationName { get; set; }
        private static readonly Dictionary<Type, ILog> Loggers = new Dictionary<Type, ILog>();
        private static readonly object Lock = new object();
        private Type _source;
        private static ILog GetLogger(Type source)
        {
            lock (Lock)
            {
                if (Loggers.ContainsKey(source))
                {
                    return Loggers[source];
                }
                var logger = LogManager.GetLogger(source);
                Loggers.Add(source, logger);
                return logger;
            }
        }

        public ILogProcessor SetSource(Type source)
        {
           _source = source;
           return this;
        }

        public ILogEntry Debug(string message, params object[] args)
        {
            var log = GetLogger(_source);    
            return new Log4NetEntry(log.Debug, log.IsDebugEnabled,ApplicationName,message,args);
        }

        public ILogEntry Info(string message, params object[] args)
        {
            var log = GetLogger(_source);
            return new Log4NetEntry(log.Info, log.IsDebugEnabled, ApplicationName, message, args);
        }

        public ILogEntry Warn(string message, params object[] args)
        {
            var log = GetLogger(_source);
            return new Log4NetEntry(log.Warn, log.IsDebugEnabled, ApplicationName, message, args);
        }

        public ILogEntry Error(string message, params object[] args)
        {
            var log = GetLogger(_source);
            return new Log4NetEntry(log.Error, log.IsDebugEnabled, ApplicationName, message, args);
        }

        public ILogEntry Fatal(string message, params object[] args)
        {
            var log = GetLogger(_source);
            return new Log4NetEntry(log.Fatal, log.IsDebugEnabled, ApplicationName, message, args);
        }
    }
}
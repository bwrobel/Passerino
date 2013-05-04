using System;
using System.Configuration;
using Passerino.Utils.ConfigurationValidator;
using Passerino.Utils.Logging;

namespace Passerino.Utils.RemoteLoggingSink.WinService
{
    public interface ILog4NetRemoteLoggingSinkConfiguration
    {
        int RemotingPort { get; }
    }

    class Log4NetRemoteLoggingSinkConfiguration : ILog4NetRemoteLoggingSinkConfiguration
    {
        private readonly IConfigManager _configManager;

        public Log4NetRemoteLoggingSinkConfiguration(IConfigManager configManager)
        {
            _configManager = configManager;
        }

        public int RemotingPort
        {
            get
            {
                return _configManager.AppSettings<int>("RemotingPort");
            }
        }
    }
}

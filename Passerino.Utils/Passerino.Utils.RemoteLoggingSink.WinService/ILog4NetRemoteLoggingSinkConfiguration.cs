using Passerino.Utils.Configuration;
using Passerino.Utils.Configuration.Management;

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
                return _configManager.GetAppSetting<int>("RemotingPort", x => (x > 0 && x <= 65536) );
            }
        }
    }
}

using System;
using System.Configuration;
using Passerino.Utils.Logging;

namespace Passerino.Utils.RemoteLoggingSink.WinService
{
    public interface ILog4NetRemoteLoggingSinkConfiguration
    {
        int RemotingPort { get; }
    }

    class Log4NetRemoteLoggingSinkConfiguration : ILog4NetRemoteLoggingSinkConfiguration
    {
        private readonly ILogProcessor _logProcessor;

        public Log4NetRemoteLoggingSinkConfiguration(ILogProcessor logProcessor)
        {
            _logProcessor = logProcessor.SetSource(GetType());
        }

        public int RemotingPort
        {
            get
            {
                var remotingPort = ConfigurationManager.AppSettings["RemotingPort"];
                int port;
                if (!Int32.TryParse(remotingPort, out port))
                {
                    var errorMessage = string.Format("Failed to parse port from appsettings/RemotingPort. Found value {0}",remotingPort);
                    _logProcessor.Fatal(errorMessage).Proceed();

                    throw new Exception(errorMessage);
                }
                return port;
            }
        }
    }
}

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.ServiceProcess;
using Passerino.Utils.Logging;

namespace Passerino.Utils.RemoteLoggingSink.WinService
{
    public partial class Log4NetRemoteLoggingSinkWinService : ServiceBase
    {
        private readonly ILogProcessor _logProcessor;
        private readonly int _remotingPort;
        private TcpChannel _channel;

        public Log4NetRemoteLoggingSinkWinService(int remotingPort ,ILogProcessor logProcessor)
        {
            InitializeComponent();

            _logProcessor = logProcessor.SetSource(GetType());
            _remotingPort = remotingPort;
        }

        protected override void OnStart(string[] args)
        {
            _channel = new TcpChannel(_remotingPort);
            ChannelServices.RegisterChannel(_channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Log4NetRemoteLoggingSink),
                typeof(Log4NetRemoteLoggingSink).Name,
                WellKnownObjectMode.Singleton);

            _logProcessor.Info("Service started, now listening for log messages on port {0}.", _remotingPort).Proceed();
        }

        protected override void OnStop()
        {
            if (_channel != null)
            {
                ChannelServices.UnregisterChannel(_channel);
                _channel = null;
            }

            _logProcessor.Info("Service stopped listening for log messages on port {0}.", _remotingPort).Proceed();
        }
    }
}

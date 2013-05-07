using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.ServiceProcess;
using Passerino.Utils.Domain;
using Passerino.Utils.Domain.Logging;

namespace Passerino.Utils.RemoteLoggingSink.WinService
{
    public partial class Log4NetRemoteLoggingSinkWinService : ServiceBase
    {
        private readonly ILog _log;
        private readonly int _remotingPort;
        private TcpChannel _channel;

        public Log4NetRemoteLoggingSinkWinService(int remotingPort ,ILogFactory logFactory)
        {
            InitializeComponent();

            _log = logFactory.New(GetType());
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

            _log.Info("Service started, now listening for log messages on port {0}.", _remotingPort).Proceed();
        }

        protected override void OnStop()
        {
            if (_channel != null)
            {
                ChannelServices.UnregisterChannel(_channel);
                _channel = null;
            }

            _log.Info("Service stopped listening for log messages on port {0}.", _remotingPort).Proceed();
        }
    }
}

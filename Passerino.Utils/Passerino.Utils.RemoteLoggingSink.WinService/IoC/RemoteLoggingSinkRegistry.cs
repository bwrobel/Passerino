using Passerino.Utils.Configuration;
using Passerino.Utils.Logging;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.RemoteLoggingSink.WinService.IoC
{
    public class RemoteLoggingSinkRegistry : Registry
    {
        public RemoteLoggingSinkRegistry()
        {
            For<Log4NetRemoteLoggingSinkWinService>().Use(() =>
                {
                    var log = ObjectFactory.GetInstance<ILogProcessor>();
                    var configManager = ObjectFactory.GetInstance<IConfigManager>();
                    var remotingPort = configManager.GetAppSetting<int>("RemotingPort", x => (x > 0 && x <= 65536));
                    return new Log4NetRemoteLoggingSinkWinService(remotingPort, log);
                });
        }
    }
}

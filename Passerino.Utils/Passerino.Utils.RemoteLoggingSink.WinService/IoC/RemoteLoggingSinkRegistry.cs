using Passerino.Utils.Caching.Redis;
using Passerino.Utils.Configuration;
using Passerino.Utils.Domain.Caching;
using Passerino.Utils.Domain.Configuration;
using Passerino.Utils.Domain.Logging;
using Passerino.Utils.Domain.Mapping;
using Passerino.Utils.Logging;
using Passerino.Utils.Mapping;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.RemoteLoggingSink.WinService.IoC
{
    public class RemoteLoggingSinkRegistry : Registry
    {
        public RemoteLoggingSinkRegistry()
        {
            For<ILogFactory>().Use(ObjectFactory.GetInstance<Log4NetLogFactory>);
            For<ICache>().Use(ObjectFactory.GetInstance<RedisCache>);
            For<IMapper>().Use(ObjectFactory.GetInstance<AutoMapperMapper>);
            For<IConfigManager>().Use(ObjectFactory.GetInstance<DefaultConfigManager>);


            For<Log4NetRemoteLoggingSinkWinService>().Use(() =>
                {
                    var logFactory = ObjectFactory.GetInstance<ILogFactory>();
                    var configManager = ObjectFactory.GetInstance<IConfigManager>();
                    var remotingPort = configManager.GetAppSetting<int>("RemotingPort", x => (x > 0 && x <= 65536));
                    return new Log4NetRemoteLoggingSinkWinService(remotingPort, logFactory);
                });
        }
    }
}

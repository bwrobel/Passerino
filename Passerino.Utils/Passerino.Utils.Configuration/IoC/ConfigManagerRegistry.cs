using Passerino.Utils.Caching.MemoryCache;
using Passerino.Utils.Configuration.AppConfig;
using Passerino.Utils.Domain.Caching;
using Passerino.Utils.Domain.Logging;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.Configuration.IoC
{
    public class ConfigManagerRegistry : Registry
    {
        public ConfigManagerRegistry()
        {
            For<DefaultConfigManager>().Use(() =>
                {
                    var logFactory = ObjectFactory.GetInstance<ILogFactory>();
                    var cacheSettings = new CacheSettings
                        {
                            DefaultCacheMinutes = AppConfigManager.Get<int>("AppConfig_CacheMinutes", x => x >= -1)
                        };

                    var cache = new MemoryCache(cacheSettings, logFactory);

                    return new DefaultConfigManager(cache, logFactory);
                });
        }

    }
}

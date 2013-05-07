using Passerino.Utils.Domain.Caching;
using Passerino.Utils.Domain.Configuration;
using ServiceStack.CacheAccess;
using ServiceStack.Redis;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.Caching.Redis.IoC
{
    public class RedisCacheRegistry : Registry
    {
        public RedisCacheRegistry()
        {

            For<ICacheSettings>().Use(() =>
                {
                    var configManager = ObjectFactory.GetInstance<IConfigManager>();
                    return new CacheSettings
                        {
                            DefaultCacheMinutes = configManager.GetAppSetting<int>("DefaultCacheMinutes", x => x >= -1)
                        };
                });

            For<IRedisClientsManager>().Use(() =>
                {
                    var configManager = ObjectFactory.GetInstance<IConfigManager>();
                    var redisHostName = configManager.GetAppSetting<string>("Redis_HostName");
                    var redisConnectTimeoutMilliseconds = 
                        configManager.GetAppSetting<int>("Redis_ConnectTimeoutMilliseconds", x => x > 0);
                    return new PooledRedisClientManager(redisHostName.Split(','))
                        {
                            ConnectTimeout = redisConnectTimeoutMilliseconds
                        };
                });
            For<ICacheClient>().Use(
                () => ObjectFactory.GetInstance<IRedisClientsManager>().GetCacheClient());
        }
    }
}

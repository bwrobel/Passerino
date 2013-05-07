using Passerino.Utils.Domain;
using Passerino.Utils.Domain.Configuration;
using ServiceStack.CacheAccess;
using ServiceStack.Redis;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.Caching.RedisDistributedMemoryCache.IoC
{
    public class RedisCacheRegistry : Registry
    {
        public RedisCacheRegistry()
        {
            For<IRedisClientsManager>().Use(() =>
            {
                var configManager = ObjectFactory.GetInstance<IConfigManager>();
                var redisHostName = configManager.GetAppSetting<string>("Redis_HostName");
                var redisConnectTimeoutMilliseconds = configManager.GetAppSetting<int>("Redis_ConnectTimeoutMilliseconds", x => x > 0);
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

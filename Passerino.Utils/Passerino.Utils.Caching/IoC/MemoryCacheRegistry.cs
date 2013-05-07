using Passerino.Utils.Caching.RedisDistributedMemoryCache;
using Passerino.Utils.Domain;
using Passerino.Utils.Domain.Caching;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.Caching.IoC
{
    public class MemoryCacheRegistry : Registry
    {
        public MemoryCacheRegistry()
        {
            //Default Cache
            For<ICache>().Use<RedisCache>();
        }
    }
}

using System;
using System.Runtime.Caching;
using Passerino.Utils.Domain.Caching;
using Passerino.Utils.Domain.Logging;

namespace Passerino.Utils.Caching.MemoryCache
{
    public class MemoryCache : BaseCache
    {
        private readonly ObjectCache _cache;
        private readonly ILog _log;

        public MemoryCache(ICacheSettings cacheSettings, ILogFactory logFactory)
            : base(cacheSettings)
        {
            _log = logFactory.New(GetType());
            _cache = System.Runtime.Caching.MemoryCache.Default;
        }

        public override void Add(CacheArguments cacheArguments, object value)
        {
            var cacheMinutes = cacheArguments.CacheMinutes ?? CacheSettings.DefaultCacheMinutes;

            var cacheItemPolicy = new CacheItemPolicy();
            if (cacheMinutes == 0) return;
            if (cacheMinutes > 0)
            {
                cacheItemPolicy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(cacheMinutes);
            }

            _cache.Set(cacheArguments.Key, value, cacheItemPolicy);

            _log.Debug("Added {0} to cache for {1} mins", cacheArguments.Key, cacheMinutes).Proceed();
        }

        public override T Get<T>(CacheArguments cacheArguments, T defaultValueIfFunctionReturnsNull = default(T))
        {
            var value = default(T);
            if(_cache.Contains(cacheArguments.Key))
            {
                value = (T) _cache[cacheArguments.Key];
            }

            return Equals(value,default(T))
                       ? defaultValueIfFunctionReturnsNull
                       : value;

        }

        public override T Get<T>(CacheArguments cacheArguments, Func<T> function, T defaultValueIfFunctionReturnsNull = default(T))
        {
            T value;
            if (_cache.Contains(cacheArguments.Key))
            {
                value = (T) _cache[cacheArguments.Key];
                _log.Debug("Got {0} from cache", cacheArguments.Key).Proceed();
            }
            else
            {
                value = function();
                Add(cacheArguments, value);
            }

            return Equals(value, default(T))
                       ? defaultValueIfFunctionReturnsNull
                       : value;
        }

        public override void Remove(string key)
        {
            if (_cache.Contains(key))
                _cache.Remove(key);
        }
    }
}

using System;

namespace Passerino.Utils.Domain.Caching
{
    public abstract class BaseCache : ICache
    {
        protected readonly ICacheSettings CacheSettings;

        protected BaseCache(ICacheSettings cacheSettings)
        {
            CacheSettings = cacheSettings;
        }

        public abstract void Add(CacheArguments cacheArguments, object value);

        public abstract T Get<T>(CacheArguments cacheArguments, T defaultValueIfFunctionReturnsNull = default(T));

        public abstract T Get<T>(CacheArguments cacheArguments, Func<T> function,T defaultValueIfFunctionReturnsNull = default(T));

        public abstract void Remove(string key);
    }
}

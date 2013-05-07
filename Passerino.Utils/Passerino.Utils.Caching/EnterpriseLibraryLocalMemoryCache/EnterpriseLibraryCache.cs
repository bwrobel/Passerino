using System;
using System.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Passerino.Utils.Domain.Caching;
using Passerino.Utils.Domain.Logging;
using CacheItemPriority = Microsoft.Practices.EnterpriseLibrary.Caching.CacheItemPriority;

namespace Passerino.Utils.Caching.EnterpriseLibraryLocalMemoryCache
{
    [Serializable]
    public class EnterpriseLibraryCache : ICache
    {
        private readonly int _defaultCacheMinutes;
        private readonly ILogProcessor _logProcessor;
        public static bool DisabledDueToTesting;
        private const string CACHE_INVALIDATOR_SESSION_KEY = "CacheStateManager";

        [NonSerialized]
        private ICacheManager _cacheManager;

        public EnterpriseLibraryCache(int defaultCacheMinutes, ILogProcessor logProcessor)
        {
            _defaultCacheMinutes = defaultCacheMinutes;
            _logProcessor = logProcessor.SetSource(GetType());
            InitializeCache();
        }


        /// <summary>
        /// The method that wraps the decorated method.
        /// This method adds the value to the cache.
        /// </summary>
        /// <param name="cacheArguments"></param>
        /// <param name="value">The value to be cached.</param>
        public void Add(CacheArguments cacheArguments, object value)
        {
            AddToCache(cacheArguments.Key, value, new TimeSpan(0, cacheArguments.CacheMinutes ?? _defaultCacheMinutes, 0));
        }

        /// <summary>
        /// The method that wraps the decorated method.
        /// This method gets a value from the cache.
        /// </summary>
        /// <param name="cacheArguments"></param>
        public T Get<T>(CacheArguments cacheArguments)
        {
            var value = GetValueFromCacheKey<T>(cacheArguments.Key);
            return value;
        }

        /// <summary>
        /// The method that wraps the decorated method.
        /// This method get's the cached value if it is cached, otherwise
        /// calls the function sent in and puts the returned value in the
        /// cache.
        /// </summary>
        /// <param name="cacheArguments"></param>
        /// <param name="func">A function returning the value to be cached.</param>
        /// <param name="defaultValueIfFunctionReturnsNull"></param>
        public T Get<T>(CacheArguments cacheArguments, Func<T> func,
            T defaultValueIfFunctionReturnsNull = default(T))
        {
            var value = GetValueFromCacheKey<T>(cacheArguments.Key);
            if (!value.Equals(default(T)))
            {
                _logProcessor.Info("Hit cache for key ({0})", cacheArguments.Key).Proceed();
                return value;
            }
            value = func();
            AddToCache(cacheArguments.Key, value, new TimeSpan(0, cacheArguments.CacheMinutes ?? _defaultCacheMinutes, 0));
            _logProcessor.Info("Added to cache for key ({0}) for {1} minutes", cacheArguments.Key,cacheArguments.CacheMinutes).Proceed();
            return value;
        }

        /// <summary>
        /// Will remove a cached value. 
        /// </summary>
        /// <param name="key">The Cache Key</param>
        public void Remove(string key)
        {
            RemoveFromCache(key);
        }


        private void InitializeCache()
        {
            if (DisabledDueToTesting) return;
            _cacheManager = CacheFactory.GetCacheManager("WebCacheManager");
        }

        private T GetValueFromCacheKey<T>(string key)
        {
            if (DisabledDueToTesting) return default(T);
            if (IsCacheStale(key))
            {
                return default(T);
            }

            return (T)_cacheManager.GetData(key);
        }

        private void AddToCache(string key, object value, TimeSpan timeSpan)
        {
            if (DisabledDueToTesting) return;

            _cacheManager.Add(key, value, CacheItemPriority.Normal, null,
                              new AbsoluteTime(timeSpan));

            if (IsCacheStale(key))
            {
                ClearStaleFlag(key);
            }
        }

        private void RemoveFromCache(string key)
        {
            var stateManager = GetStateManager();
            stateManager.InvalidateCache(key);
        }

        private static CacheStateManager GetStateManager()
        {
            var memoryCache = MemoryCache.Default;

            var cacheInvalidator = (CacheStateManager)memoryCache.Get(CACHE_INVALIDATOR_SESSION_KEY);
            if (cacheInvalidator == null)
            {
                cacheInvalidator = new CacheStateManager();
                memoryCache.Add(CACHE_INVALIDATOR_SESSION_KEY, cacheInvalidator,
                                new CacheItemPolicy
                                {
                                    AbsoluteExpiration = DateTime.Now.AddSeconds(600)
                                });
            }
            return cacheInvalidator;
        }

        private static bool IsCacheStale(string key)
        {
            var stateManager = GetStateManager();
            var machineName = Environment.MachineName;

            return stateManager.IsCacheStale(key, machineName);
        }

        private static void ClearStaleFlag(string key)
        {
            var stateManager = GetStateManager();
            var machineName = Environment.MachineName;

            stateManager.ClearStaleFlag(key, machineName);
        }
    }
}

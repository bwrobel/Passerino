using System;
using Passerino.Utils.Domain.Caching;
using Passerino.Utils.Domain.Logging;
using Passerino.Utils.Extensions.DateTimeExtensions;
using ServiceStack.CacheAccess;

namespace Passerino.Utils.Caching.RedisDistributedMemoryCache
{
    public class RedisCache : ICache
    {
        internal static int FailsInARow = 0;
        internal static DateTime LastFail = DateTime.MinValue;

        private readonly ILogProcessor _logProcessor;
        private readonly object _lockObject = new object();
        private readonly ICacheClient _cacheClient;
        private readonly IDateTimeExtension _dateTimeExtension;
        private readonly int _defaultCacheMinutes;

        public RedisCache(ICacheClient cacheClient, IDateTimeExtension dateTimeExtension, int  defaultCacheMinutes, ILogProcessor logProcessor)
        {
            _cacheClient = cacheClient;
            _dateTimeExtension = dateTimeExtension;
            _defaultCacheMinutes = defaultCacheMinutes;
            _logProcessor = logProcessor.SetSource(GetType());
        }

        internal void FailedCallingTheServer()
        {
            lock (_lockObject)
            {
                LastFail = _dateTimeExtension.UtcNow;
                FailsInARow++;
            }
        }

        internal void SucceededCallingTheServer()
        {
            lock (_lockObject)
            {
                FailsInARow = 0;
            }
        }

        internal bool ShouldUseCache()
        {
            if (FailsInARow > 9)
            {
                _logProcessor.Fatal("ShouldUseCache(...) - Redis has failed {0} times in a row!", FailsInARow).Proceed();
                var lastFailMoreThanFiveMinutesOld = LastFail < _dateTimeExtension.UtcNow.AddMinutes(-5);
                return (lastFailMoreThanFiveMinutesOld);
            }
            return true;
        }

        public void Add(CacheArguments cacheArguments, object value)
        {
            if (!ShouldUseCache()) return;
            try
            {
                _cacheClient.Set(cacheArguments.Key, value,
                                 new TimeSpan(0, 0, cacheArguments.CacheMinutes ?? _defaultCacheMinutes, 0));
                SucceededCallingTheServer();
                _logProcessor.Warn("Add(...) - Added {0} to cache for {1} mins", cacheArguments.Key, cacheArguments.CacheMinutes).Proceed();
            }
            catch (Exception ex)
            {
                FailedCallingTheServer();
                _logProcessor.Error("Add(...) - Redis seems to be down!").WithException(ex).Proceed();
            }
        }

        public T Get<T>(CacheArguments cacheArguments)
        {
            if (!ShouldUseCache()) return default(T);
            try
            {
                var value = _cacheClient.Get<T>(cacheArguments.Key);
                if (!value.Equals(default(T)))
                {
                    _logProcessor.Info("Hit Redis cache for key ({0})", cacheArguments.Key).Proceed();
                }
                SucceededCallingTheServer();
                return value;
            }
            catch (Exception ex)
            {
                FailedCallingTheServer();
                _logProcessor.Error("Get(...) - Redis seems to be down!").WithException(ex).Proceed();
            }
            return default(T);
        }

        public T Get<T>(CacheArguments cacheArguments, Func<T> function,
            T defaultValueIfFunctionReturnsNull = default(T))
        {
            var value = Get<T>(cacheArguments);
            if (!value.Equals(default(T)))
            {
                return value;
            }
            value = function();
            Add(cacheArguments, value);
            return value.Equals(default(T)) ? defaultValueIfFunctionReturnsNull : value;
        }

        public void Remove(string key)
        {
            if (!ShouldUseCache()) return;
            try
            {
                _cacheClient.Remove(key);
                SucceededCallingTheServer();
                _logProcessor.Info("Removed key ({0}) from Redis.", key).Proceed();
            }
            catch (Exception ex)
            {
                FailedCallingTheServer();
                _logProcessor.Error("Add(...) - Redis seems to be down!").WithException(ex).Proceed();
            }
        }
    }
}
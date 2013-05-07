using System;
using Passerino.Utils.Domain.Caching;
using Passerino.Utils.Domain.Logging;
using Passerino.Utils.Extensions.DateTimeExtensions;
using ServiceStack.CacheAccess;

namespace Passerino.Utils.Caching.Redis
{
    public class RedisCache : BaseCache
    {
        internal static int FailsInARow = 0;
        internal static DateTime LastFail = DateTime.MinValue;

        private readonly ILog _log;
        private readonly object _lockObject = new object();
        private readonly ICacheClient _cacheClient;
        private readonly IDateTimeExtension _dateTimeExtension;

        public RedisCache(ICacheClient cacheClient, IDateTimeExtension dateTimeExtension, ICacheSettings cacheSettings, ILogFactory logFactory)
            : base(cacheSettings)
        {
            _cacheClient = cacheClient;
            _dateTimeExtension = dateTimeExtension;
            _log = logFactory.New(GetType());
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
                _log.Fatal("ShouldUseCache(...) - Redis has failed {0} times in a row!", FailsInARow).Proceed();
                var lastFailMoreThanFiveMinutesOld = LastFail < _dateTimeExtension.UtcNow.AddMinutes(-5);
                return (lastFailMoreThanFiveMinutesOld);
            }
            return true;
        }

        public override void Add(CacheArguments cacheArguments, object value)
        {
            if (!ShouldUseCache()) return;
            try
            {
                _cacheClient.Set(cacheArguments.Key, value,new TimeSpan(0, 0, cacheArguments.CacheMinutes ?? CacheSettings.DefaultCacheMinutes, 0));
                SucceededCallingTheServer();
                _log.Warn("Add(...) - Added {0} to cache for {1} mins", cacheArguments.Key, cacheArguments.CacheMinutes).Proceed();
            }
            catch (Exception ex)
            {
                FailedCallingTheServer();
                _log.Error("Add(...) - Redis seems to be down!").WithException(ex).Proceed();
            }
        }

        public override T Get<T>(CacheArguments cacheArguments, T defaultValueIfFunctionReturnsNull = default(T))
        {
            if (!ShouldUseCache()) return default(T);
            try
            {
                var value = _cacheClient.Get<T>(cacheArguments.Key);
                if (!Equals(value, default(T)))
                {
                    _log.Info("Hit Redis cache for key ({0})", cacheArguments.Key).Proceed();
                }
                SucceededCallingTheServer();
                return value;
            }
            catch (Exception ex)
            {
                FailedCallingTheServer();
                _log.Error("Get(...) - Redis seems to be down!").WithException(ex).Proceed();
            }
            return defaultValueIfFunctionReturnsNull;
        }

        public override T Get<T>(CacheArguments cacheArguments, Func<T> function,
            T defaultValueIfFunctionReturnsNull = default(T))
        {
            var value = Get<T>(cacheArguments);
            if (!Equals(value, default(T)))
            {
                return value;
            }
            value = function();
            Add(cacheArguments, value);
            return Equals(value, default(T)) ? defaultValueIfFunctionReturnsNull : value;
        }

        public override void Remove(string key)
        {
            if (!ShouldUseCache()) return;
            try
            {
                _cacheClient.Remove(key);
                SucceededCallingTheServer();
                _log.Info("Removed key ({0}) from Redis.", key).Proceed();
            }
            catch (Exception ex)
            {
                FailedCallingTheServer();
                _log.Error("Add(...) - Redis seems to be down!").WithException(ex).Proceed();
            }
        }
    }
}
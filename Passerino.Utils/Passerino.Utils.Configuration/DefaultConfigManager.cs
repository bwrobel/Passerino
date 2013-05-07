using System;
using System.Collections.Generic;
using Passerino.Utils.Caching.MemoryCache;
using Passerino.Utils.Configuration.AppConfig;
using Passerino.Utils.Domain.Caching;
using Passerino.Utils.Domain.Configuration;
using Passerino.Utils.Domain.Logging;

namespace Passerino.Utils.Configuration
{
    public class DefaultConfigManager : IConfigManager
    {
        private readonly ICache _cache;
        private readonly ILog _log;
        private static readonly Dictionary<string,object> AppSettingsValues = new Dictionary<string, object>(); 

        public DefaultConfigManager(ICache cache, ILogFactory logFactory)
        {
            _log = logFactory.New(GetType());
            _cache = cache;
            
        }

        public T GetAppSetting<T>(string key, Func<T, bool> valueValidator = null)
        {
            if (AppSettingsValues.ContainsKey(key))
                return (T)AppSettingsValues[key];

            try
            {
                return _cache.Get(new CacheArguments(key), () => AppConfigManager.Get(key, valueValidator));
            }
            catch (Exception ex)
            {
                _log.Fatal("Cannot get App Settings with key \"{0}\"", key).WithException(ex).Proceed();
                throw;
            }
            
        }


    }


}

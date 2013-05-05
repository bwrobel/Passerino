using System;
using Passerino.Utils.Configuration.AppConfig;
using Passerino.Utils.Logging;

namespace Passerino.Utils.Configuration
{
    public class ConfigManager : IConfigManager
    {
        private readonly ILogProcessor _logProcessor;

        public ConfigManager(ILogProcessor logProcessor)
        {
            _logProcessor = logProcessor.SetSource(GetType());
        }

        public T GetAppSetting<T>(string key, Func<T, bool> valueValidator = null)
        {
            try
            {
                return AppConfigManager.Get(key, valueValidator);
            }
            catch (Exception ex)
            {
                _logProcessor.Fatal("Cannot get App Settings with key \"{0}\"", key).WithException(ex).Proceed();
                throw;
            }
            
        }


    }


}

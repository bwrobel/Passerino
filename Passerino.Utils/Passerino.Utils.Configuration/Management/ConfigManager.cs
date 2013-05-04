using System;
using Passerino.Utils.Configuration.Management.AppConfig;

namespace Passerino.Utils.Configuration.Management
{
    public class ConfigManager : IConfigManager
    {
        public T GetAppSetting<T>(string key, Func<T, bool> valueValidator = null)
        {
            return AppConfigManager.Get(key, valueValidator);
        }


    }


}

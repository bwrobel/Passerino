using System;

namespace Passerino.Utils.Configuration
{
    public interface IConfigManager
    {
        T GetAppSetting<T>(string key, Func<T, bool> valueValidator = null);
    }
}
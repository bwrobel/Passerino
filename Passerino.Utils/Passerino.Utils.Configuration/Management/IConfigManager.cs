using System;

namespace Passerino.Utils.Configuration.Management
{
    public interface IConfigManager
    {
        T GetAppSetting<T>(string key, Func<T, bool> valueValidator = null);
    }
}
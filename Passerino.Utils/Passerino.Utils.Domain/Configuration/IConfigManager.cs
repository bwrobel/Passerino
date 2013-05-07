using System;

namespace Passerino.Utils.Domain.Configuration
{
    public interface IConfigManager
    {
        T GetAppSetting<T>(string key, Func<T, bool> valueValidator = null);
    }
}
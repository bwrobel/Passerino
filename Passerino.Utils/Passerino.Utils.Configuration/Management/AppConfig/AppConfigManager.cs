using System;
using System.Configuration;

namespace Passerino.Utils.Configuration.Management.AppConfig
{
    public static class AppConfigManager
    {
        public static T Get<T>(string key, Func<T, bool> valueValidator)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("AppSettings Key is invalid", "key");

            var stringValue = ConfigurationManager.AppSettings[key];
            if (stringValue == null)
            {
                throw new ConfigurationErrorsException(string.Format("AppSettings Value for key \"{0}\" not provided", key));
            }

            T castedValue;
            try
            {
                castedValue = (T)Convert.ChangeType(stringValue, typeof(T));
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(string.Format("AppSettings Value \"{0}\" for key \"{1}\" cannot be cast to type \"{2}\"", stringValue, key, typeof(T).FullName), ex);
            }

            bool castedValueIsValid;
            try
            {
                castedValueIsValid = valueValidator == null || valueValidator(castedValue);
            }
            catch (Exception)
            {
                castedValueIsValid = false;
            }

            if (!castedValueIsValid)
            {
                throw new ConfigurationErrorsException(string.Format("AppSettings Value \"{0}\" for key \"{1}\" not validate", stringValue, key, valueValidator));
            }

            return castedValue;
        }

    }
}

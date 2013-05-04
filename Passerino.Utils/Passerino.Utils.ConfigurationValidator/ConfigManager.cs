using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Passerino.Utils.ConfigurationValidator
{
    public interface IConfigManager
    {
        T AppSettings<T>(string key, string regexPattern = null);
    }

    public class ConfigManager : IConfigManager
    {
        public T AppSettings<T>(string key, string regexPattern = null)
        {
            if(string.IsNullOrWhiteSpace(key)) throw new ArgumentException("AppSettings Key is invalid", "key");

            var stringValue = ConfigurationManager.AppSettings[key];
            if (stringValue == null)
            {
                throw new ConfigurationErrorsException(string.Format("AppSettings Value for key \"{0}\" not provided", key));
            }
            
            if (regexPattern != null && Regex.IsMatch(stringValue,regexPattern))
            {
                throw new ConfigurationErrorsException(string.Format("AppSettings Value \"{0}\" for key \"{1}\" not match regexPattern \"{2}\"", stringValue, key, regexPattern));
            }

            try
            {
                return  (T)Convert.ChangeType(stringValue, typeof(T));
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException(string.Format("AppSettings Value \"{0}\" for key \"{1}\" cannot be cast to type \"{2}\"", stringValue, key, typeof(T).FullName),ex);
            }
        }
    }


}

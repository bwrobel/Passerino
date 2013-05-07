namespace Passerino.Utils.Domain.Caching
{
    public class CacheArguments
    {
        public CacheArguments(string key, int? cacheMinutes = null)
        {
            Key = key;
            CacheMinutes = cacheMinutes;
        }

        /// <summary>
        /// Edge values : 
        /// - equal 0 => no cache
        /// - less than 0 => infinite
        /// </summary>
        public int? CacheMinutes { get; private set; } 
        public string Key { get; private set; }
    }
}
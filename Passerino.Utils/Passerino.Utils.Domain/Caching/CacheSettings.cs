namespace Passerino.Utils.Domain.Caching
{
    public interface ICacheSettings
    {
        int DefaultCacheMinutes { get; set; }
    }

    public class CacheSettings : ICacheSettings
    {
        public int DefaultCacheMinutes { get; set; }    
    }
}
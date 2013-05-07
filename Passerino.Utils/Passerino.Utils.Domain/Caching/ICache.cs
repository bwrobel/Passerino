using System;

namespace Passerino.Utils.Domain.Caching
{
    public interface ICache
    {
        void Add(CacheArguments cacheArguments, object value);
        T Get<T>(CacheArguments cacheArguments, T defaultValueIfFunctionReturnsNull = default(T));
        
        /// If the func returns null one can add default value so that the
        /// cache will be hit next time, instead of a time-consuming database
        /// call.
        T Get<T>(CacheArguments cacheArguments, Func<T> function, T defaultValueIfFunctionReturnsNull = default(T));
        void Remove(string key);
    }
}
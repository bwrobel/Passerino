using Microsoft.Practices.EnterpriseLibrary.Caching;
using Passerino.Utils.Domain.Configuration;
using Passerino.Utils.Domain.Logging;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.Caching.EnterpriseLibrary.IoC
{
    public class EnterpriseLibraryCacheRegistry : Registry
    {
        public EnterpriseLibraryCacheRegistry()
        {
            For<ICacheManager>().Use(CacheFactory.GetCacheManager("WebCacheManager"));
        }

    }
}

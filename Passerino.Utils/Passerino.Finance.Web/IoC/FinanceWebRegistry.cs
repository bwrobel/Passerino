using Passerino.Utils.Caching.Redis;
using Passerino.Utils.Configuration;
using Passerino.Utils.Domain.Caching;
using Passerino.Utils.Domain.Configuration;
using Passerino.Utils.Domain.Logging;
using Passerino.Utils.Domain.Mapping;
using Passerino.Utils.Logging;
using Passerino.Utils.Mapping;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Finance.Web.IoC
{
    public class FinanceWebRegistry : Registry
    {
        public FinanceWebRegistry()
        {
            For<ILogFactory>().Use(ObjectFactory.GetInstance<Log4NetLogFactory>);
            For<ICache>().Use(ObjectFactory.GetInstance<RedisCache>);
            For<IMapper>().Use(ObjectFactory.GetInstance<AutoMapperMapper>);
            For<IConfigManager>().Use(ObjectFactory.GetInstance<DefaultConfigManager>);
        }
    }
}
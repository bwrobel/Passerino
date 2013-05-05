using Passerino.Utils.Logging;
using Passerino.Utils.Logging.Log4Net;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.Configuration.Management.Log4NetConfig
{
    public class Log4NetRegistry : Registry
    {
        public Log4NetRegistry()
        {
            For<ILogProcessor>().Use<Log4NetProcessor>();
        }
    }
}

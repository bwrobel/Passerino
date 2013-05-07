using StructureMap.Configuration.DSL;

namespace Passerino.Utils.Logging.IoC
{
    public class Log4NetRegistry : Registry
    {
        public Log4NetRegistry()
        {
            For<Log4NetLogFactory>().Singleton().Use<Log4NetLogFactory>();
        }

        public static void PostInitialize()
        {
            Log4NetLogFactory.Initialize();
        }
    }
}

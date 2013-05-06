using System;
using Passerino.Utils.IoC.StructureMap;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.IoC.IoC
{
    public class IoCRegistry : Registry
    {
        public IoCRegistry()
        {
            //Scanning all default instances 
            //TODO:Verify if necessary
            Scan(scan =>
            {
                scan.AssembliesFromPath(Environment.CurrentDirectory, AssemblyIoCVisibleAttribute.AssemblyVisible);
                scan.WithDefaultConventions();
            });
        }

        

        public static void PostInitialize()
        {
            //structure map configuration validation
            ObjectFactory
                .GetInstance<StructureMapConfigManager>()
                .AssertConfigurationIsValid();
        }
    }
}

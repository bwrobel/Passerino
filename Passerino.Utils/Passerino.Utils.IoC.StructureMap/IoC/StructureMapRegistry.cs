using System;
using Passerino.Utils.Domain.IoC;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.IoC.IoC
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
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

using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Passerino.Utils.Configuration.Validation.StructureMap;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.Configuration.Management.StructureMapConfig
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {
            //Scanning all default instances
            //Scan(scan =>
            //{
            //    scan.AssembliesFromPath(Environment.CurrentDirectory, StructureMapConfigManager.IsIoCVisibleAssembly);
            //    scan.WithDefaultConventions();
            //});
        }

        

        public static void PostInitialize()
        {
            //structure map configuration validation
            ObjectFactory
                .GetInstance<StructureMapValidator>()
                .AssertConfigurationIsValid();
        }
    }
}

using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.Mappers;
using Passerino.Utils.Configuration.StructureMapConfig;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.Configuration.AutoMapperConfig
{
    public class AutoMapperRegistry : Registry  
    {
        public AutoMapperRegistry()
        {
            //Auto Mapper init
            For<ConfigurationStore>().Singleton().Use<ConfigurationStore>()
               .Ctor<IEnumerable<IObjectMapper>>().Is(MapperRegistry.AllMappers());
            For<IConfigurationProvider>().Use(ctx => ctx.GetInstance<ConfigurationStore>());
            For<IConfiguration>().Use(ctx => ctx.GetInstance<ConfigurationStore>());
            For<ITypeMapFactory>().Use<TypeMapFactory>();
            For<IMappingEngine>().Use<MappingEngine>();

            Scan(scan =>
            {
                scan.AssembliesFromPath(Environment.CurrentDirectory, StructureMapConfigManager.AssemblyIsIoCVisible);
                scan.AddAllTypesOf<Profile>();
                scan.WithDefaultConventions();
            });

            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });

        }

        public static void PostInitialize()
        {
            ObjectFactory.GetInstance<AutoMapperConfigManager>().Initialize();

            ObjectFactory.GetInstance<AutoMapperValidator>().AssertConfigurationIsValid();
        }
    }
}

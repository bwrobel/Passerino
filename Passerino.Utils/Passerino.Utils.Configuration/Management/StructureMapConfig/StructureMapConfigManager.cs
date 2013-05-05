using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.Configuration.Management.StructureMapConfig
{
    public static class StructureMapConfigManager
    {
        public static void Initialize()
        {
            InitializeAllApplicationRegistries();

            ExecutePostInitializeEvents();
        }

        private static void InitializeAllApplicationRegistries()
        {
            ObjectFactory.Configure(cfg => cfg.Scan(scan =>
            {
                scan.AssembliesFromPath(Environment.CurrentDirectory, IsIoCVisibleAssembly);
                scan.LookForRegistries();
                scan.WithDefaultConventions();
            }));
        }

        public static bool IsIoCVisibleAssembly(Assembly assembly)
        {
            var customAttributes = assembly.GetCustomAttributes(typeof(AssemblyIoCVisibleAttribute), false);
            return customAttributes.Any(a => ((AssemblyIoCVisibleAttribute)a).Visible);
        }

        private static void ExecutePostInitializeEvents()
        {
            foreach (var registry in GetAllApplicationRegistries())
            {
                var postInitializeEvent = registry.GetMethod("PostInitialize");
                if (postInitializeEvent != null)
                {
                    postInitializeEvent.Invoke(null, null);
                }
                    
            }
        }

        private static IEnumerable<Type> GetAllApplicationRegistries()
        {
            var registries = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                registries
                    .AddRange(
                        assembly.GetTypes()
                                .Where(x => x.IsSubclassOf(typeof(Registry)))
                                .ToList()
                    );
            }
            return registries;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Passerino.Utils.Domain.IoC;
using Passerino.Utils.Domain.Logging;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.IoC
{
    public class StructureMapConfigManager : IIoCConfigManager
    {
        private readonly ILog _log;

        public StructureMapConfigManager(ILogFactory logFactory)
        {
            _log = logFactory.New(GetType());
        }

        public void Initialize()
        {
            InitializeAllApplicationRegistries();

            ExecutePostInitializeEvents();
        }

        public void AssertConfigurationIsValid()
        {
            try
            {
                ObjectFactory.AssertConfigurationIsValid();
                _log.Info("StructureMap Configuration Validation Succeded").Proceed();
            }
            catch (Exception ex)
            {
                _log.Fatal("StructureMap Configuration Validation Failed").WithException(ex).Proceed();
                throw;
            }
        }

        private void InitializeAllApplicationRegistries()
        {
            _log.Info("Initialize All Application Registries Started").Proceed();


            ObjectFactory.Configure(cfg => cfg.Scan(scan =>
            {
                scan.AssembliesFromPath(Environment.CurrentDirectory, AssemblyIoCVisibleAttribute.AssemblyVisible);
                scan.LookForRegistries();
                scan.WithDefaultConventions();
            }));

            _log.Info("Initialize All Application Registries Finished").Proceed();

        }

        private void ExecutePostInitializeEvents()
        {
                        
            foreach (var registry in GetAllApplicationRegistries())
            {
                var postInitializeEvent = registry.GetMethod("PostInitialize");
                if (postInitializeEvent != null)
                {
                    try
                    {
                        postInitializeEvent.Invoke(null, null);
                        _log.Info("Post Initialize Event for Registry \"{0}\" succeded", registry.FullName).Proceed();
                    }
                    catch (Exception ex)
                    {
                        _log.Fatal("Post Initialize Event for Registry \"{0}\" failed",registry.FullName).WithException(ex).Proceed();
                        throw;
                    }
                    
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

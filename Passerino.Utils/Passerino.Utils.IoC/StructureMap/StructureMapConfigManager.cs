using System;
using System.Collections.Generic;
using System.Linq;
using Passerino.Utils.Logging;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.IoC.StructureMap
{
    public class StructureMapConfigManager : IIoCConfigManager
    {
        private readonly ILogProcessor _logProcessor;

        public StructureMapConfigManager(ILogProcessor logProcessor)
        {
            _logProcessor = logProcessor.SetSource(GetType());
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
                _logProcessor.Info("StructureMap Configuration Validation Succeded").Proceed();
            }
            catch (Exception ex)
            {
                _logProcessor.Fatal("StructureMap Configuration Validation Failed").WithException(ex).Proceed();
                throw;
            }
        }

        private void InitializeAllApplicationRegistries()
        {
            _logProcessor.Info("Initialize All Application Registries Started").Proceed();


            ObjectFactory.Configure(cfg => cfg.Scan(scan =>
            {
                scan.AssembliesFromPath(Environment.CurrentDirectory, AssemblyIoCVisibleAttribute.AssemblyVisible);
                scan.LookForRegistries();
                scan.WithDefaultConventions();
            }));

            _logProcessor.Info("Initialize All Application Registries Finished").Proceed();

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
                        _logProcessor.Info("Post Initialize Event for Registry \"{0}\" succeded", registry.FullName).Proceed();
                    }
                    catch (Exception ex)
                    {
                        _logProcessor.Fatal("Post Initialize Event for Registry \"{0}\" failed",registry.FullName).WithException(ex).Proceed();
                        throw;
                    }
                    
                }
                    
            }
        }

        private IEnumerable<Type> GetAllApplicationRegistries()
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

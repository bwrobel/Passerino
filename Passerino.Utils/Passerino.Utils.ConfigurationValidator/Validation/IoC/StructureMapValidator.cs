using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Passerino.Utils.Logging;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Passerino.Utils.ConfigurationValidator.Validation.IoC
{
    public class StructureMapValidator : IConfigurationValidator
    {
        private readonly ILogProcessor _logProcessor;

        public StructureMapValidator(ILogProcessor logProcessor)
        {
            _logProcessor = logProcessor.SetSource(GetType());
        }

        public bool AssertConfigurationIsValid()
        {
            var configurationIsValid = true;
            foreach (var registry in ObjectFactory.GetAllInstances<Registry>())
            {
                try
                {
                    ObjectFactory.Initialize(init => init.AddRegistry(registry));
                    ObjectFactory.AssertConfigurationIsValid();
                }
                catch (Exception ex)
                {
                    
                    _logProcessor
                        .Fatal("AssertConfiguration Failed for Registry \"{0}\"",registry.GetType().FullName)
                        .WithException(ex)
                        .Proceed();

                    configurationIsValid = false;
                }
            }
            return configurationIsValid;
        }
    }
}

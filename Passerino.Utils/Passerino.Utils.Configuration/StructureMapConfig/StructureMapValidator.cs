using System;
using Passerino.Utils.Configuration.Log4NetConfig;
using Passerino.Utils.Logging;
using StructureMap;

namespace Passerino.Utils.Configuration.StructureMapConfig
{
    public class StructureMapValidator : IConfigurationValidator
    {
        private readonly ILogProcessor _logProcessor;

        public StructureMapValidator()
        {
            _logProcessor = Log4NetConfigManager
                .GetInstance(Environment.CurrentDirectory)
                .SetSource(GetType());
        }

        public void AssertConfigurationIsValid()
        {
            try
            {
                ObjectFactory.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                    
                _logProcessor
                    .Fatal("StructureMap Configuration Validation Failed")
                    .WithException(ex)
                    .Proceed();
                throw;
            }
        }
    }
}

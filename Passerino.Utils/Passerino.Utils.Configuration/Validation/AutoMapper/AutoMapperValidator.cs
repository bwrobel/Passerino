using System;
using AutoMapper;
using Passerino.Utils.Logging;
using StructureMap;

namespace Passerino.Utils.Configuration.Validation.AutoMapper
{
    public class AutoMapperValidator : IConfigurationValidator
    {
        private readonly ILogProcessor _logProcessor;

        public AutoMapperValidator(ILogProcessor logProcessor)
        {
            _logProcessor = logProcessor.SetSource(GetType());
        }

        public bool AssertConfigurationIsValid()
        {
            var configurationIsValid = true;
            foreach (var profile in ObjectFactory.GetAllInstances<Profile>())
            {
                try
                {
                    Mapper.AddProfile(profile);
                    Mapper.AssertConfigurationIsValid();
                    Mapper.Reset();
                }
                catch (Exception ex)
                {
                    _logProcessor
                        .Fatal("AssertConfiguration Failed for Profile \"{0}\"", profile.GetType().FullName)
                        .WithException(ex)
                        .Proceed();

                    configurationIsValid = false;
                }
                
            }

            return configurationIsValid;
        }
    }
}

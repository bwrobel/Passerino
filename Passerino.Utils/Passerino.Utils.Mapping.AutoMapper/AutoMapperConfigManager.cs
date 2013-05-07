using System;
using AutoMapper;
using Passerino.Utils.Domain.Logging;
using StructureMap;

namespace Passerino.Utils.Mapping
{
    public class AutoMapperConfigManager
    {
        private readonly ILog _log;

        public AutoMapperConfigManager(ILogFactory logFactory)
        {
            _log = logFactory.New(GetType());
        }

        public void Initialize()
        {
            try
            {
                var configuration = ObjectFactory.GetInstance<IConfiguration>();
                foreach (var profile in ObjectFactory.GetAllInstances<Profile>())
                {
                    configuration.AddProfile(profile);
                }
                _log.Info("Auto Mapper Config Manager Successfuly initialized all Profiles").Proceed();
            }
            catch (Exception ex)
            {
                _log.Fatal("Auto Mapper Config Manager Failed to initialize all Profiles")
                             .WithException(ex)
                             .Proceed();
                throw;
            }
            
        }

        public void AssertConfigurationIsValid()
        {
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
                    _log
                        .Fatal("AssertConfiguration Failed for Profile \"{0}\"", profile.GetType().FullName)
                        .WithException(ex)
                        .Proceed();

                    throw;
                }
            }
        }
    }
}

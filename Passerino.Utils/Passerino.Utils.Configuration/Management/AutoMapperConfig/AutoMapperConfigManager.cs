using System;
using AutoMapper;
using Passerino.Utils.Logging;
using StructureMap;

namespace Passerino.Utils.Configuration.Management.AutoMapperConfig
{
    public class AutoMapperConfigManager
    {
        private readonly ILogProcessor _logProcessor;

        public AutoMapperConfigManager(ILogProcessor logProcessor)
        {
            _logProcessor = logProcessor.SetSource(GetType());
        }

        public void Initialize()
        {
            //try
            //{
            //    var configuration = ObjectFactory.GetInstance<IConfiguration>();
            //    foreach (var profile in ObjectFactory.GetAllInstances<Profile>())
            //    {
            //        configuration.AddProfile(profile);
            //    }
            //    _logProcessor.Info("Auto Mapper Config Manager Successfuly initialized all Profiles").Proceed();
            //}
            //catch (Exception ex)
            //{
            //    _logProcessor.Fatal("Auto Mapper Config Manager Failed to initialize all Profiles")
            //                 .WithException(ex)
            //                 .Proceed();
            //    throw;
            //}
            
        }
    }
}

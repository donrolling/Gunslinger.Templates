using Common.Logging;
using idi.sample.test.Injection;
using idi.sample.test.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using System;
using System.IO;

namespace idi.sample.test.Base
{
    public class DataAccessTestBase
    {
        public TestConfiguration TestConfiguration { get; }
        public IServiceProvider ServiceProvider { get; }
        public ILogger Logger { get; }

        public DataAccessTestBase()
        {
            this.ServiceProvider = Setup();
            this.Logger = LogUtility.GetLogger(this.ServiceProvider, this.GetType());
            this.TestConfiguration = this.ServiceProvider.GetService<TestConfiguration>();
        }

        private static IServiceProvider Setup()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var testConfiguration = getJSONConfigFile<TestConfiguration>($"{ currentDirectory }\\TestConfiguration.json");

            var services = new ServiceCollection();
            services.AddLogging(a => a.AddNLog());
            services.AddSingleton(testConfiguration);
            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            DataAccess.AddServices(testConfiguration.ConnectionString, loggerFactory, services);

            serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        private static T getJSONConfigFile<T>(string path) where T : class
        {
            var fileContents = File.ReadAllText(path);
            var result = JsonConvert.DeserializeObject<T>(fileContents);
            return result;
        }
    }
}
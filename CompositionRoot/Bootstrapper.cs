using Gunslinger.DataProviders;
using Gunslinger.Engines;
using Gunslinger.Facades;
using Gunslinger.Factories;
using Gunslinger.Factories.SQL;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NLog.Extensions.Logging;
using System.IO;

namespace CompositionRoot
{
    public class Bootstrapper
    {
        public static ServiceProvider ConfigureServices(GenerationContext generationContext)
        {
            var services = new ServiceCollection();
            services.AddLogging(a => a.AddNLog());
            services.AddSingleton(generationContext);
            services.AddTransient<IGeneratorFacade, GeneratorFacade>();
            services.AddTransient<IModelGeneratorFacade, ModelGeneratorFacade>();
            services.AddTransient<ITemplateOutputEngine, FileOutputEngine>();
            services.AddTransient<IResourceOutputEngine, ResourceOutputEngine>();
            services.AddTransient<Gunslinger.Interfaces.IFileProvider, FileTemplateProvider>();
            services.AddTransient<IRenderEngine, HandlebarsRenderEngine>();
            //services.AddTransient<IRenderEngine, StubbleRenderEngine>();
            services.AddTransient<IContextFactory, GenerationContextFactory>();
            services.AddTransient<IDataProviderFactory, DataProviderFactory>();
            services.AddTransient<ISQLServerInfoFactory, SQLServerInfoFactory>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        public static T GetJSONConfigFile<T>(string path) where T : class
        {
            var fileContents = File.ReadAllText(path);
            var result = JsonConvert.DeserializeObject<T>(fileContents);
            return result;
        }
    }
}
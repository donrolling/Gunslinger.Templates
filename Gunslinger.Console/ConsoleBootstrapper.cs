using CompositionRoot;
using Gunslinger.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Gunslinger.Console
{
    public class ConsoleBootstrapper
    {
        public static IServiceProvider Bootstrap(string pathToGenerationContext, string outputPath)
        {
            var generationContext = ConfigurationRetriever.GetGenerationConfiguration(pathToGenerationContext);
            generationContext.OutputDirectory = outputPath;
            var serviceProvider = Bootstrapper.ConfigureServices(generationContext);
            return serviceProvider;
        }
    }
}
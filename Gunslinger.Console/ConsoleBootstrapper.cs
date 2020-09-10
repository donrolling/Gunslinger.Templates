using CompositionRoot;
using Gunslinger.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Gunslinger.Console
{
    public class ConsoleBootstrapper
    {
        public static IServiceProvider Bootstrap(string pathToGenerationContext, string outputPath, string templateDirectory, bool processTemplateStubs)
        {
            var generationContext = ConfigurationRetriever.GetGenerationConfiguration(pathToGenerationContext);
            generationContext.OutputDirectory = outputPath;
            generationContext.TemplateDirectory = templateDirectory;
            generationContext.ProcessTemplateStubs = processTemplateStubs;
            var serviceProvider = Bootstrapper.ConfigureServices(generationContext);
            return serviceProvider;
        }
    }
}